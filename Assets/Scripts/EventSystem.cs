using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class EventSystem : MonoBehaviour {

    //fade immediate in/out to specific value with specific name
    public enum typeOfEvent { Audio, Anim, Mouse, Create, Destroy, NextTest, Reset, jumpTo,
                                wait, fade, loop, callFunction, printCurrent, printForExamples,
                                resetTimer, stopTimer, printScores, recordAudio, stopRecording, move };
    public TestHandler testHandler;
    public static int totalLooped;
    AudioClip aud;
    public int loopTimes = 2;
    public AudioClip reminder;
    float waitTimer = 0.0f;
    public static bool paused = false;

    [System.Serializable]
    public struct Anim
    {
        public string targetPos;
        public string originalPos;
        public int step;
    }

    [System.Serializable]
    public struct Create
    {
        public string targetPos;
        public bool hasParent;
        public bool useOriginSize;
        public bool reverseScaleFromParent;
        public int layer;
        public string parent;
        public string name;
        public float percentOfHeightScale;
        public GameObject obj;
    }

    [System.Serializable]
    public struct CallFunction
    {
        public string functionName;
        public string objectName;
        public string scriptName;
    }

    [System.Serializable]
    public struct Fading
    {
        public string layer;
        public float fadeTo;
        public float delta;
    }

    [System.Serializable]
    public struct DestroyObj
    {
        public string name;
    }

    [System.Serializable]
    public struct AudioRecording
    {
        public string fileName;
    }

    [System.Serializable]
    public class EventObject
    {
        public typeOfEvent type;
        public AudioClip audio;
        public Anim anim;
        public Create create;
        public DestroyObj destroy;
        public CallFunction callFunction;
        public Fading fading;
        public AudioRecording record;
        public string jumpLabel;
        public float wait;
        public int loopTo;
        public bool destroyOnFinish;
    }

    public GameObject GetObject(string name)
    {
        return GameObject.Find(name);
    }

    public bool RunEvent(EventObject e)
    {
        bool done = false;
        testHandler.backEndItem.currentEvent = e;
        switch (e.type)
        {
            case typeOfEvent.Anim:
                if (GetObject(e.anim.originalPos).transform.position.x != GetObject(e.anim.targetPos).transform.position.x ||
                    GetObject(e.anim.originalPos).transform.position.y != GetObject(e.anim.targetPos).transform.position.y)
                {
                    float step;
                    if (e.anim.step == 0)
                        step = 2.0f * Time.deltaTime;
                    else
                        step = e.anim.step * Time.deltaTime;
                    GetObject(e.anim.originalPos).transform.position = Vector2.MoveTowards(GetObject(e.anim.originalPos).transform.position, GetObject(e.anim.targetPos).transform.position, step);
                }
                else
                {
                    done = true;
                }
                break;
            case typeOfEvent.Audio:
                GetComponent<AudioSource>().UnPause();
                if (!GetComponent<AudioSource>().clip)
                {
                    GetComponent<AudioSource>().clip = e.audio;
                    GetComponent<AudioSource>().pitch = 1f;
                    GetComponent<AudioSource>().Play();
                }
                if (!GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<AudioSource>().clip = null;
                    done = true;
                }
                break;
            case typeOfEvent.Mouse:
                done = testHandler.mouseIsDone;
                break;
            case typeOfEvent.Create:
                GameObject holder;
                if (e.create.hasParent)
                    holder = Instantiate(e.create.obj, GetObject(e.create.parent).transform);
                else
                    holder = Instantiate(e.create.obj);
                holder.transform.position = GetObject(e.create.targetPos).transform.position;
                holder.name = e.create.name;
                if(e.create.percentOfHeightScale > 0)
                {
                    float fullScreenHeight = Camera.main.orthographicSize * 2;
                    Vector3 newSize = Camera.main.WorldToScreenPoint(Vector3.one * (e.create.percentOfHeightScale * fullScreenHeight));
                    //newSize.x = newSize.y;
                    newSize = newSize - Camera.main.WorldToScreenPoint(Vector3.zero);
                    holder.GetComponent<RectTransform>().sizeDelta = newSize;
                }
                if(e.create.useOriginSize)
                {
                    holder.transform.localScale = GetObject(e.create.targetPos).transform.localScale;
                }
                if(e.create.reverseScaleFromParent && e.create.hasParent)
                {
                    holder.transform.localScale = Vector3.one/GetObject(e.create.parent).transform.localScale.x;
                }
                if(holder.GetComponent<SpriteRenderer>())
                {
                    SpriteRenderer sr = holder.GetComponent<SpriteRenderer>();
                    sr.sortingLayerID = e.create.layer;
                }
                done = true;
                break;
            case typeOfEvent.Destroy:
                string[] names = e.destroy.name.Split(',');
                foreach(string name in names)
                    GameObject.Destroy(GetObject(name));
                done = true;
                break;
            case typeOfEvent.NextTest:
                //flag in testhandler, if dirty then go to next test
                totalLooped = 0;
                testHandler.nextTest = true;
                break;
            case typeOfEvent.jumpTo:
                done = true;
                break;
            case typeOfEvent.wait:
                waitTimer += Time.deltaTime;
                if(waitTimer >= e.wait)
                {
                    waitTimer = 0.0f;
                    done = true;
                }
                break;
            case typeOfEvent.fade:
                GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]; //will return an array of all GameObjects in the scene
                bool keepGoing = true;
                foreach (GameObject go in gos)
                {
                    if (go.layer == LayerMask.NameToLayer(e.fading.layer))
                    {
                        if(go.GetComponent<SpriteRenderer>())
                        {
                            Color t = go.GetComponent<SpriteRenderer>().color;

                            //make this a param of event
                            t.a = Mathf.MoveTowards(t.a, e.fading.fadeTo, e.fading.delta);
                            go.GetComponent<SpriteRenderer>().color = t;
                            if (Mathf.Abs(t.a - e.fading.fadeTo) >= .01f)
                                keepGoing = false;
                        }
                        else if(go.GetComponent<TMP_Text>())
                        {
                            Color t = go.GetComponent<TMP_Text>().color;
                            //make this a param of event
                            t.a = Mathf.MoveTowards(t.a, e.fading.fadeTo, e.fading.delta);
                            go.GetComponent<TMP_Text>().color = t;
                            if (Mathf.Abs(t.a - e.fading.fadeTo) >= .01f)
                                keepGoing = false;
                        }
                    }
                }
                done = keepGoing;
                break;
            //tag all fadeable objects, fade over x seconds
            case typeOfEvent.loop:
                print(totalLooped);
                if (totalLooped != loopTimes)
                {
                    testHandler.backEndItem.eventNumber = e.loopTo;
                    totalLooped++;
                }
                else
                {
                    totalLooped = 0;
                    SceneHandler.GoToNextScene();
                }
                //testHandler.currentTestNumber = e.loopTo;
                break;

            case typeOfEvent.printCurrent:
                //OutputHandler.PrintCurrent(testHandler.testAbbrev);
                done = true;
                break;

            case typeOfEvent.printForExamples:
                print(loopTimes + " " + totalLooped);
                //for (int i = 0; i < loopTimes - totalLooped; i++)
                //    OutputHandler.PrintNA(testHandler.testAbbrev);
                totalLooped = 0;
                done = true;
                break;

            case typeOfEvent.callFunction:
                print(e.callFunction.objectName + " " + e.callFunction.scriptName + " " + e.callFunction.functionName);
                GameObject o = GameObject.Find(e.callFunction.objectName);
                Component c = o.GetComponent(e.callFunction.scriptName);
                print(c.GetType().ToString() + " " + o.name);
                MethodInfo mi = c.GetType().GetMethod(e.callFunction.functionName);
                mi.Invoke(c, null);
                done = true;
                break;

            case typeOfEvent.resetTimer:
                OutputHandler.ResetTimer();
                testHandler.reminded = false;
                done = true;
                break;

            case typeOfEvent.stopTimer:
                OutputHandler.timer.Stop();
                done = true;
                break;

            case typeOfEvent.printScores:
                //ScoreReports.PrintScores(testHandler.testAbbrev);
                done = true;
                break;

            case typeOfEvent.recordAudio:
                //start microphone on first call, on second call set time stamp
                //on stop recording get audio from start to stop time stamp
                //thats it
                aud = Microphone.Start("", true, 120, 44100);
                done = true;
                //aud.Play();
                break;

            case typeOfEvent.stopRecording:
                Microphone.End("");
                //Capture the current clip data
                //AudioClip recordedClip = aud;
                //var position = Microphone.GetPosition("Built-in Microphone");
                ////this is the per audio file length
                //print((float)position);
                //print(position * recordedClip.channels + (44100 * 2));
                //var soundData = new float[position * recordedClip.channels + (44100 * 2)];
                //recordedClip.GetData(soundData, 0);

                ////Create shortened array for the data that was used for recording
                //var newData = new float[position * recordedClip.channels + (44100 * 2)];

                ////Microphone.End (null);
                ////Copy the used samples to a new array
                //for (int i = 0; i < newData.Length; i++)
                //{
                //    newData[i] = soundData[i];
                //}

                //GetComponent<AudioSource>().clip = aud;
                //GetComponent<AudioSource>().Play();
                ////Invoke("PlayAudio", 7);

                ////One does not simply shorten an AudioClip,
                ////    so we make a new one with the appropriate length
                ////print(position + " " + recordedClip.frequency + " " + position/recordedClip.frequency);
                //var newClip = AudioClip.Create(recordedClip.name, position + (2*44100), recordedClip.channels, recordedClip.frequency, false);
                //newClip.SetData(soundData, 0);        //Give it the data from the old clip

                //print(recordedClip.length + " " + newClip.length);

                ////Replace the old clip
                //AudioClip.Destroy(recordedClip);
                //aud = newClip;

                SavWav.Save(e.record.fileName, aud, testHandler.testAbbrev);
                done = true;
                break;

            case typeOfEvent.move:
                GetObject(e.anim.originalPos).transform.position = GetObject(e.anim.targetPos).transform.position;
                done = true;
                break;
        }
        return done;
    }

    public void PlayAudio()
    {
        GetComponent<AudioSource>().clip = aud;
        GetComponent<AudioSource>().Play();
    }

    public void Pausing()
    {
        paused = !paused;

        EventObject t = new EventObject();
        List<EventObject> holder = testHandler.backEndItem.events.ToList();

        if (paused)
        {
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().clip = null;
            holder.Insert(testHandler.backEndItem.eventNumber, t);
            testHandler.backEndItem.events = holder.ToArray();
        }
        else
        {
            holder.RemoveAt(testHandler.backEndItem.eventNumber);
            testHandler.backEndItem.events = holder.ToArray();
        }
    }

    public void ReminderAudio()
    {
        EventObject t = new EventObject();
        t.audio = reminder;
        t.type = typeOfEvent.Audio;
        t.destroyOnFinish = true;
        List<EventObject> holder = testHandler.backEndItem.events.ToList();

        holder.Insert(testHandler.backEndItem.eventNumber, t);
        testHandler.backEndItem.events = holder.ToArray();
    }

    public void DestroyCurrentEvent()
    {

        EventObject t = new EventObject();
        List<EventObject> holder = testHandler.backEndItem.events.ToList();

        holder.RemoveAt(testHandler.backEndItem.eventNumber);
        testHandler.backEndItem.events = holder.ToArray();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if(!hasFocus)
        {
            if(testHandler.backEndItem.events[testHandler.backEndItem.eventNumber].type == typeOfEvent.Audio)
            {
                GetComponent<AudioSource>().clip = null;
                GetComponent<AudioSource>().Stop();
            }
        }
    }

    private void Update()
    {
        if(!paused)
        {
            if (RunEvent(testHandler.backEndItem.events[testHandler.backEndItem.eventNumber]))
            {
                if (testHandler.backEndItem.events[testHandler.backEndItem.eventNumber].destroyOnFinish)
                    DestroyCurrentEvent();
                else
                    testHandler.backEndItem.eventNumber++;
            }
        }
        else
        {
            GetComponent<AudioSource>().Pause();
        }
    }
}
