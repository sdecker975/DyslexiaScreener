using UnityEngine;
using System.Collections;
using UnityEngine.UI; 
using System.IO;

public class settingsMenu : MonoBehaviour{

	public enum saveFormat {playerprefs, iniFile};
	public saveFormat saveAs;

    public bool pauseTimeWhenMenuOpen;//if Checked in inspector - Sets TimeScale to 0 when menu is open.

    [Header("THESE NEED TO BE DRAGGED IN")]
    //if you use the prefab "_QualitySettingsMenu" they should all be assigned for you;
    public Slider qualityLevelSlider;
    public Slider antiAliasSlider, shadowResolutionSlider, textureQualitySlider, anisotropicModeSlider, anisotropicLevelSlider;
	public Text qualityText, antiAliasText, shadowText, textureText, anisotropicModeText, anisotropicLevelText, fpsCounterText;
	public GameObject resolutionsPanel, resButtonPrefab, menuTransform;
	public Text currentResolutionText;
	public Toggle FPSToggle, windowedModeToggle, vSyncToggle;

	private GameObject resolutionsPanelParent;
	private Camera canvasCamera;
	private Resolution[] resolutions;
	private string[] outPut=new string[10], splitLine=new string[2], inPut=new string[10];
	private string lineToRead;
	private int lineCounter;

	private bool setMenu, openMenu, showFPS, fullScreenMode, toggleVSync;

	private const float fpsMeasurePeriod = 0.2f;
	private float fpsNextPeriod = 0;
	private int fpsAccumulator = 0, currentFps, wantedResX, wantedResY;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(transform.gameObject);
		fpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
		resolutionsPanelParent=resolutionsPanel.transform.parent.parent.gameObject;

		//this reads all the values of the sliders and toggles and sets the Graphic settings accordingly.
		//(if the settings were saved before, they wil all be set to the saved setting before reading them)
		//(if this is the first time the game starts the toggles and sliders wil be where they were when the game was build)
		//(if you want the game to start at certain settings the first time, make sure to set everyting before you build)
		SetValues();
	}
	
	// Update is called once per frame
	void Update () {
					
		if (Input.GetKeyDown(KeyCode.Escape))//the Key used to open this menu.
		{
			//if you want to use a UI button to open the menu put this function on it.
			OpenQualitySettingMenu();
		}
		
		if (openMenu)
		{
			if (!setMenu)
			{
			    menuTransform.gameObject.SetActive(true); 
				setMenu=true;

				if (pauseTimeWhenMenuOpen)
					Time.timeScale=0;
			}
		}
		else 
		{
			if (!setMenu)
			{
                menuTransform.gameObject.SetActive(false);
				SavePlayerprefs();
				setMenu=true;

				if (pauseTimeWhenMenuOpen)
					Time.timeScale=1;
			}
		}

        if (setTextQual)
        {
            if (setTextQualDelay <= 0)
            {
                switch (Mathf.RoundToInt(textureQualitySlider.value))
                {
                    case 0:
                        QualitySettings.masterTextureLimit = 3;
                        break;
                    case 1:
                        QualitySettings.masterTextureLimit = 2;
                        break;
                    case 2:
                        QualitySettings.masterTextureLimit = 1;
                        break;
                    case 3:
                        QualitySettings.masterTextureLimit = 0;
                        break;
                }
                setTextQual = false;
            }
            else setTextQualDelay -= Time.deltaTime;
        }

        //this FPScounter is a standard Unity asset (thought it was handy to put it in).
        if (showFPS)
		{
			fpsAccumulator++;
			if (Time.realtimeSinceStartup > fpsNextPeriod)
			{
				currentFps = (int) (fpsAccumulator/fpsMeasurePeriod);
				fpsAccumulator = 0;
				fpsNextPeriod += fpsMeasurePeriod;
                fpsCounterText.text = "FPS:" + currentFps;
            }			
		}
		else fpsCounterText.text="";
	}
	
	public void OpenQualitySettingMenu() //opens the menu.
	{
		openMenu=!openMenu;
		setMenu=false;
	}

	public void SetQuality() //changes the general Quality setting without changing the Vsync,Antialias or Anisotropic settings.
	{
		int graphicSetting=Mathf.RoundToInt(qualityLevelSlider.value);
		QualitySettings.SetQualityLevel(graphicSetting,true);
		qualityText.text=QualitySettings.names[graphicSetting];
		//keep settings the way the Sliders and Toggels are set.
		SetWindowedMode();
		SetVSync();
		SetAntiAlias();
        SetShadowResolution();
        SetTextureQuality();
        SetAnisotropicFiltering();
		SetAnisotropicFilteringLevel();
	}
	
	public void ShowFPS()
	{
		showFPS = !showFPS;
	}

	public void SetWindowedMode()
	{
		if (windowedModeToggle.isOn)
			fullScreenMode=false;		
		else fullScreenMode=true;
		Screen.SetResolution(wantedResX,wantedResY,fullScreenMode);
	}

	public void SetVSync()
	{
        if(vSyncToggle)
        {
            if (vSyncToggle.isOn)
                QualitySettings.vSyncCount = 1;
            else QualitySettings.vSyncCount = 0;
        }
	}

	public void SetAntiAlias()
	{
        if(antiAliasSlider)
        {
            int sliderValue = Mathf.RoundToInt(antiAliasSlider.value);
            switch (sliderValue)
            {
                case 0:
                    QualitySettings.antiAliasing = 0;
                    antiAliasText.text = "Off";
                    break;
                case 1:
                    QualitySettings.antiAliasing = 2;
                    antiAliasText.text = QualitySettings.antiAliasing.ToString() + "x Multi Sampling";
                    break;
                case 2:
                    QualitySettings.antiAliasing = 4;
                    antiAliasText.text = QualitySettings.antiAliasing.ToString() + "x Multi Sampling";
                    break;
                case 3:
                    QualitySettings.antiAliasing = 8;
                    antiAliasText.text = QualitySettings.antiAliasing.ToString() + "x Multi Sampling";
                    break;
            }
        }
	}

    public void SetShadowResolution()
    {
        if(shadowResolutionSlider)
        {
            switch (Mathf.RoundToInt(shadowResolutionSlider.value))
            {
                case 0:
                    QualitySettings.shadowResolution = ShadowResolution.Low;
                    shadowText.text = "Low";
                    break;
                case 1:
                    QualitySettings.shadowResolution = ShadowResolution.Medium;
                    shadowText.text = "Medium";
                    break;
                case 2:
                    QualitySettings.shadowResolution = ShadowResolution.High;
                    shadowText.text = "High";
                    break;
                case 3:
                    QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
                    shadowText.text = "VeryHigh";
                    break;
            }
        }
    }

    bool setTextQual;
    float setTextQualDelay;
    public void SetTextureQuality()
    {
        if(textureQualitySlider)
        {
            setTextQualDelay = 0.1f;
            setTextQual = true;

            //Adjusting the Texture Quality here may cause problems when the slider is changed really fast.
            //moved to Update.
            switch (Mathf.RoundToInt(textureQualitySlider.value))
            {
                case 0:
                    //QualitySettings.masterTextureLimit = 3;
                    textureText.text = "Eighth Res";
                    break;
                case 1:
                    //QualitySettings.masterTextureLimit = 2;
                    textureText.text = "Quarter Res";
                    break;
                case 2:
                    //QualitySettings.masterTextureLimit = 1;
                    textureText.text = "Half Res";
                    break;
                case 3:
                    //QualitySettings.masterTextureLimit = 0;
                    textureText.text = "Full Res";
                    break;
            }
        }
    }

    public void SetAnisotropicFiltering()
	{
        if(anisotropicModeSlider)
        {
            switch (Mathf.RoundToInt(anisotropicModeSlider.value))
            {
                case 0:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                    anisotropicModeText.text = "Disabled";
                    break;
                case 1:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                    anisotropicModeText.text = "Enabled";
                    break;
                case 2:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                    anisotropicModeText.text = "ForceEnabled";
                    break;
            }
        }
	}

	public void SetAnisotropicFilteringLevel()
	{
        if(anisotropicLevelSlider)
        {
            int SliderValue = Mathf.RoundToInt(anisotropicLevelSlider.value);
            Texture.SetGlobalAnisotropicFilteringLimits(SliderValue, SliderValue);
            anisotropicLevelText.text = SliderValue.ToString();
        }
	}

	public void QuitGame()
	{
		SavePlayerprefs();
		Application.Quit();
	}

	private void SetValues()//set all settings according to the menu buttons.
	{
		//his reads how many Quality levels your "Game" has and sices the slider accordingly.
		qualityLevelSlider.maxValue=QualitySettings.names.Length-1;

		resolutions=Screen.resolutions;//checking the available resolution options.
        //filling the Screen Resolution option menu with buttons, one for every available resolution option your monitor has.
        int prefResX = 0;
        int prefRezY = 0;

		for (int i=0; i<resolutions.Length; i++)
		{
            if (resolutions[i].width != prefResX && resolutions[i].height != prefRezY)//prevent creating duplicate resolution buttons.
            {
              GameObject button = Instantiate(resButtonPrefab);//the button prefab.
              button.GetComponentInChildren<Text>().text = resolutions[i].width + "x" + resolutions[i].height;
              int index = i;
              button.GetComponent<Button>().onClick.AddListener(() => { SetResolution(index); });//adding a "On click" SetResolution() function to the button.
              button.transform.SetParent(resolutionsPanel.transform, false);

              prefResX = resolutions[i].width;
              prefRezY = resolutions[i].height;
            }
		}

		LoadPlayerprefs(); // if any settings were saved before, this is where they are loaded and Sliders and toggles are set to the saved position.

		//reading Sliders and toggles and setting everything accordingly.
		int graphicSetting=Mathf.RoundToInt(qualityLevelSlider.value);
		QualitySettings.SetQualityLevel(graphicSetting,true);
		qualityText.text=QualitySettings.names[graphicSetting];
		SetVSync();
		SetWindowedMode();
		SetAntiAlias();
        SetShadowResolution();
        SetTextureQuality();
        SetAnisotropicFiltering();
		SetAnisotropicFilteringLevel();
	}

	public void SetResolution(int index)//the "On click" function on the resolutions buttons.
	{	
		wantedResX=resolutions[index].width;
		wantedResY=resolutions[index].height;
		Screen.SetResolution(wantedResX,wantedResY,fullScreenMode);
		currentResolutionText.text=wantedResX+"x"+wantedResY;
	}

	public void ShowResolutionOptions()//opens the dropdown menu with available resolution options.
	{
		if (resolutionsPanelParent.activeSelf==false)
			resolutionsPanelParent.SetActive(true);
		else resolutionsPanelParent.SetActive(false);
	}

	private void SavePlayerprefs()
	{
		if (saveAs==saveFormat.playerprefs)
		{
		    PlayerPrefs.SetInt("prefsSaved",1);
		    
            if(qualityLevelSlider)
		        PlayerPrefs.SetInt("graphicsSlider",Mathf.RoundToInt(qualityLevelSlider.value));
            if(antiAliasSlider)
		        PlayerPrefs.SetInt("antiAliasSlider",Mathf.RoundToInt(antiAliasSlider.value));
            if(shadowResolutionSlider)
                PlayerPrefs.SetInt("shadowResolutionSlider", Mathf.RoundToInt(shadowResolutionSlider.value));
            if(textureQualitySlider)
                PlayerPrefs.SetInt("textureQualitySlider",Mathf.RoundToInt(textureQualitySlider.value));
            if(anisotropicModeSlider)
                PlayerPrefs.SetInt("anisotropicModeSlider",Mathf.RoundToInt(anisotropicModeSlider.value));
            if(anisotropicLevelSlider)
		        PlayerPrefs.SetInt("anisotropicLevelSlider",Mathf.RoundToInt(anisotropicLevelSlider.value));
		    
		    PlayerPrefs.SetInt("wantedResolutionX",wantedResX);
		    PlayerPrefs.SetInt("wantedResolutionY",wantedResY);
		    
		    int toggle = 0;
		    if (!showFPS)
		    	toggle=0;
		    else toggle=1;
		    PlayerPrefs.SetInt("FPSToggle",toggle);
		    
            if(vSyncToggle)
            {
                if (vSyncToggle.isOn)
                    toggle = 1;
                else toggle = 0;
                PlayerPrefs.SetInt("vSyncToggle", toggle);
            }
		    
            if(windowedModeToggle)
            {
                if (windowedModeToggle.isOn)
                    toggle = 1;
                else toggle = 0;
                PlayerPrefs.SetInt("windowedModeToggle", toggle);
            }
		}
		else if (saveAs==saveFormat.iniFile)
		{
		    StreamWriter wr = new StreamWriter(Application.dataPath+"/QualitySettings.ini");

            if(qualityLevelSlider)
            {
                string graphicsSliderV = Mathf.RoundToInt(qualityLevelSlider.value).ToString();
                outPut[0] = string.Format("Quality level={0}", graphicsSliderV);
            }
            if(antiAliasSlider)
            {
                string antiAliasSliderV = Mathf.RoundToInt(antiAliasSlider.value).ToString();
                outPut[1] = string.Format("Anti Alias level={0}", antiAliasSliderV);
            }
            if(shadowResolutionSlider)
            {
                string shadowResolutionSliderV = Mathf.RoundToInt(shadowResolutionSlider.value).ToString();
                outPut[8] = string.Format("Shadow Resolution={0}", shadowResolutionSliderV);
            }
            if(textureQualitySlider)
            {
                string textureQualitySliderV = Mathf.RoundToInt(textureQualitySlider.value).ToString();
                outPut[9] = string.Format("Texture Quality={0}", textureQualitySliderV);
            }
            if(anisotropicModeSlider)
            {
                string anisotropicModeSliderV = Mathf.RoundToInt(anisotropicModeSlider.value).ToString();
                outPut[2] = string.Format("Anisotropic Mode={0}", anisotropicModeSliderV);
            }
            if(anisotropicLevelSlider)
            {
                string anisotropicLevelSliderV = Mathf.RoundToInt(anisotropicLevelSlider.value).ToString();
                outPut[3] = string.Format("Anisotropic Level={0}", anisotropicLevelSliderV);
            }

            string wantedResolutionX=wantedResX.ToString();
			string wantedResolutionY=wantedResY.ToString();

            int toggle = 0;
			if (!showFPS)
				toggle=0;
			else toggle=1;
			outPut[4]=string.Format("Show FPS={0}",toggle);

            if(windowedModeToggle)
            {
                if (windowedModeToggle.isOn)
                    toggle = 1;
                else toggle = 0;
                outPut[5] = string.Format("Windowed Mode={0}", toggle);
            }

            if(vSyncToggle)
            {
                if (vSyncToggle.isOn)
                    toggle = 1;
                else toggle = 0;
                outPut[6] = string.Format("V Sync={0}", toggle);
            }

			outPut[7]=string.Format("Resolution(widthxheight)={0}x{1}",wantedResolutionX,wantedResolutionY);

			for (int i=0;i<outPut.Length; i++)
			{
				wr.WriteLine(outPut[i]);	
			}
		    wr.Close();		    
		}
	}

	private void LoadPlayerprefs()
	{
		if (saveAs==saveFormat.playerprefs)
		{
		    if (PlayerPrefs.GetInt("prefsSaved")==1)//to check if there are any.
		    {
                if(qualityLevelSlider)
		    	    qualityLevelSlider.value=PlayerPrefs.GetInt("graphicsSlider");
                if(antiAliasSlider)
		            antiAliasSlider.value=PlayerPrefs.GetInt("antiAliasSlider");
                if(shadowResolutionSlider)
                    shadowResolutionSlider.value = PlayerPrefs.GetInt("shadowResolutionSlider");
                if(textureQualitySlider)
                    textureQualitySlider.value = PlayerPrefs.GetInt("textureQualitySlider");
                if(anisotropicModeSlider)
                    anisotropicModeSlider.value=PlayerPrefs.GetInt("anisotropicModeSlider");
                if(anisotropicLevelSlider)
		            anisotropicLevelSlider.value=PlayerPrefs.GetInt("anisotropicLevelSlider");
		        
		    	wantedResX=PlayerPrefs.GetInt("wantedResolutionX");
		    	wantedResY=PlayerPrefs.GetInt("wantedResolutionY");
		    	currentResolutionText.text=wantedResX+"x"+wantedResY;
		    
		    	int toggle = PlayerPrefs.GetInt("FPSToggle");
		    	if (toggle==1)
		    	{
		    		FPSToggle.isOn=true;
		    		showFPS=true;
		    	}
		    	else 
		    	{
		    		FPSToggle.isOn=false;
		    		showFPS=false;
		    	}
		    
		        toggle = PlayerPrefs.GetInt("windowedModeToggle");
		        if (toggle==1)
		       	    windowedModeToggle.isOn=true;		
		        else windowedModeToggle.isOn=false;
                
                if(vSyncToggle)
                {
                    toggle = PlayerPrefs.GetInt("vSyncToggle");
                    if (toggle == 1)
                        vSyncToggle.isOn = true;
                    else vSyncToggle.isOn = false;
                }
		    }
		    else //no player prefs are saved.
		    {
		    	//if nothing was saved use the full screen resolutions
		    	wantedResX=Screen.width;
		    	wantedResY=Screen.height;
                currentResolutionText.text = Screen.width + "x" + Screen.height;//sets the text of the Screen Resolution button to the res we start with.
            }
		}
		else if (saveAs==saveFormat.iniFile)
		{
			if (System.IO.File.Exists(Application.dataPath+"/QualitySettings.ini"))//to check if there are any.
			{
				StreamReader sr = new StreamReader(Application.dataPath+"/QualitySettings.ini");

				lineCounter=0;
				while ((lineToRead=sr.ReadLine())!=null)
				{
					splitLine=lineToRead.Split('=');
					inPut[lineCounter]=splitLine[1];	
					lineCounter++;
				}
				sr.Close();

				qualityLevelSlider.value=int.Parse(inPut[0]);
				antiAliasSlider.value=int.Parse(inPut[1]);
				anisotropicModeSlider.value=int.Parse(inPut[2]);
				anisotropicLevelSlider.value=int.Parse(inPut[3]);
                shadowResolutionSlider.value=int.Parse(inPut[8]);
                textureQualitySlider.value=int.Parse(inPut[9]);

                splitLine=inPut[7].Split('x');
				wantedResX=int.Parse(splitLine[0]);
				wantedResY=int.Parse(splitLine[1]);
				currentResolutionText.text=splitLine[0]+"x"+splitLine[1];
				
				int toggle = int.Parse(inPut[4]);
				if (toggle==1)
				{
					FPSToggle.isOn=true;
					showFPS=true;
				}
				else 
				{
					FPSToggle.isOn=false;
					showFPS=false;
				}
				
				toggle = int.Parse(inPut[5]);
				if (toggle==1)
					windowedModeToggle.isOn=true;		
				else windowedModeToggle.isOn=false;
				
				toggle=int.Parse(inPut[6]);
				if (toggle==1)
					vSyncToggle.isOn=true;
				else vSyncToggle.isOn=false;
			}
			else //no player prefs are saved.
			{
				//if nothing was saved use the full screen resolutions
				wantedResX=Screen.width;
				wantedResY=Screen.height;
                currentResolutionText.text = Screen.width + "x" + Screen.height;
            }
		}
	}

	//for testing/Debugging.
	public void DeletePlayerprefs()
	{
		PlayerPrefs.DeleteKey("prefsSaved");
		PlayerPrefs.DeleteKey("FPSToggle");
		PlayerPrefs.DeleteKey("graphicsSlider");
		PlayerPrefs.DeleteKey("antiAliasSlider");
        PlayerPrefs.DeleteKey("shadowResolutionSlider");
        PlayerPrefs.DeleteKey("textureQualitySlider");
        PlayerPrefs.DeleteKey("anisotropicModeSlider");
		PlayerPrefs.DeleteKey("anisotropicLevelSlider");		
		PlayerPrefs.DeleteKey("wantedResolutionX");
		PlayerPrefs.DeleteKey("wantedResolutionY");		
		PlayerPrefs.DeleteKey("windowedModeToggle");		
		PlayerPrefs.DeleteKey("vSyncToggle"); 
	}
}
