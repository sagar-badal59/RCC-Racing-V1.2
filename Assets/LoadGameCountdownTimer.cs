using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

    /// <summary>This is a basic, network-synced CountdownTimer based on properties.</summary>
    /// <remarks>
    /// In order to start the timer, the MasterClient can call SetStartTime() to set the timestamp for the start.
    /// The property 'StartTime' then contains the server timestamp when the timer has been started.
    /// 
    /// In order to subscribe to the CountdownTimerHasExpired event you can call CountdownTimer.OnCountdownTimerHasExpired
    /// += OnCountdownTimerIsExpired;
    /// from Unity's OnEnable function for example. For unsubscribing simply call CountdownTimer.OnCountdownTimerHasExpired
    /// -= OnCountdownTimerIsExpired;.
    /// 
    /// You can do this from Unity's OnEnable and OnDisable functions.
    /// </remarks>
    public class LoadGameCountdownTimer : MonoBehaviourPunCallbacks
{
    /// <summary>
    ///     OnCountdownTimerHasExpired delegate.
    /// </summary>
    public delegate void CountdownTimerHasExpired();

    public const string LoadGameCountdownStartTime = "LoadGameTime";

    [Header("Countdown time in seconds")]
    public float Countdown = 5.0f;

    private bool isTimerRunning;

    private int startTime;

    [Header("Reference to a Text component for visualizing the countdown")]
    public Text Text;
    public Text timerText;
    public Text playerCount;

  
    public int WAIT_TIME_IN_SEC;


    /// <summary>
    ///     Called when the timer has expired.
    /// </summary>
    public static event CountdownTimerHasExpired OnCountdownTimerHasExpired;


    public void Start()
    {
        if (this.Text == null) Debug.LogError("Reference to 'Text' is not set. Please set a valid reference.", this);
    }

    public override void OnEnable()
    {
        Debug.Log("OnEnable CountdownTimer");
        base.OnEnable();

        // the starttime may already be in the props. look it up.
        Initialize();
    }

    public override void OnDisable()
    {
        //base.OnDisable();
        Debug.Log("OnDisable CountdownTimer");
    }


    public void Update()
    {
        if (!this.isTimerRunning) return;
        int countdown = (int)TimeRemaining();
        timerText.text = "Remaining Time : "+countdown.ToString();
        Debug.Log("Remaining Time " + countdown);
        string addString = ".";
        if ((int)countdown % 3 == 0)
            addString = "...";
        else if ((int)countdown % 3 == 2)
            addString = ".";
        else if ((int)countdown % 3 == 1)
            addString = "..";

        this.Text.text = "Waiting for Other Players "+addString;

        if (countdown > 0.0f) return;
        
        OnTimerEnds();
    }


    private void OnTimerRuns()
    {
        this.isTimerRunning = true;
        this.enabled = true;
    }

    private void OnTimerEnds()
    {
        if (!this.enabled) return;
        timerText.text = "";
        this.isTimerRunning = false;
        this.enabled = false;

        Debug.Log("Emptying info text.", this.Text);
        this.Text.text = string.Empty;
        this.Text.text = "Loading...";


        if (PhotonNetwork.PlayerList.Length >= Constants.MINIMUM_PLAYER)
        {
            //if (PhotonNetwork.IsMasterClient)
            //{
            //    Hashtable props = new Hashtable();
            //    props.Remove(LoadGameCountdownStartTime);
            //    PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            //}
            if (OnCountdownTimerHasExpired != null) OnCountdownTimerHasExpired();
        }
        else
        {
            Debug.Log("Player is less than minimum player");
            if (PhotonNetwork.IsMasterClient)
            {
                Hashtable props = new Hashtable();
                props.Remove(LoadGameCountdownStartTime);
                PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                SetStartTime();
            }
        }
    }


    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        Debug.Log("CountdownTimer.OnRoomPropertiesUpdate " + propertiesThatChanged.ToStringFull());
        //if (this.isTimerRunning)
        //    {
        //        if(!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(LoadGameCountdownStartTime))
        //        OnTimerEnds();
        //    }
        //    else
        if (!this.isTimerRunning && PhotonNetwork.IsMasterClient && !PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(GameStartCountdownTimer.GameStartCountdownStartTime))
        {
                Initialize();
            }
        
    }

    public override void OnJoinedRoom()
    {
        playerCount.text = " Total Player " + PhotonNetwork.PlayerList.Length;
        if (PhotonNetwork.IsMasterClient)
        {
            SetStartTime();
        }
        else
        {
            Initialize();
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerCount.text = " Total Player " + PhotonNetwork.PlayerList.Length;
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("New Player Joined ");
    }


    private void Initialize()
    {
        int propStartTime;
        if (TryGetStartTime(out propStartTime))
        {
            this.startTime = propStartTime;
            Debug.Log("Initialize sets StartTime " + this.startTime + " server time now: " + PhotonNetwork.ServerTimestamp + " remain: " + TimeRemaining());


            this.isTimerRunning = TimeRemaining() > 0;

            if (this.isTimerRunning)
                OnTimerRuns();
            else
                OnTimerEnds();
        }
    }


    private float TimeRemaining()
    {
        int timer = PhotonNetwork.ServerTimestamp - this.startTime;
        return this.Countdown - timer / 1000f;
    }


    public static bool TryGetStartTime(out int startTimestamp)
    {
        startTimestamp = PhotonNetwork.ServerTimestamp;
        if (!PhotonNetwork.IsConnectedAndReady) return false;

        object startTimeFromProps;
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(LoadGameCountdownStartTime) && PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(LoadGameCountdownStartTime, out startTimeFromProps))
        {
            startTimestamp = (int)startTimeFromProps;
            print(startTimestamp);
            return true;
        }

        return false;
    }


    public static void SetStartTime()
    {
        int startTime = 0;
        bool wasSet = TryGetStartTime(out startTime);

        Hashtable props = new Hashtable
            {
                {LoadGameCountdownStartTime, (int)PhotonNetwork.ServerTimestamp}
            };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);


        Debug.Log("Set Custom Props for Time: " + props.ToStringFull() + " wasSet: " + wasSet);
    }
    
}
