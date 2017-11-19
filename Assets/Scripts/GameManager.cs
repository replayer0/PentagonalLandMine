using UnityEngine;
using System.IO;
using System;

public class CustomLogHandler : ILogHandler
{
    private FileStream m_FileStream = null;
    private StreamWriter m_streamWriter = null;
    private ILogHandler m_logHandler = Debug.unityLogger.logHandler;

    public CustomLogHandler()
    {
        var now = DateTime.Now;
        string filePath = string.Format("{0}/{1}_{2}_{3}{4}", Application.persistentDataPath, now.Year, now.Month, now.Day, ".txt");

        m_FileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        m_streamWriter = new StreamWriter(m_FileStream);

        // Replace the default debug log handler
        Debug.unityLogger.logHandler = this;
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        string log = string.Format("[{0}][{1}] {2} ", DateTime.Now, logType.ToString(), format);

        // write stream
        m_streamWriter.WriteLine(String.Format(format, args));
        m_streamWriter.Flush();

        // write log
        m_logHandler.LogFormat(logType, context, log, args);
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
        m_logHandler.LogException(exception, context);
    }
}

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// DEFINE
    /// </summary>
    public enum STAGE_ENUM
    {
        DEFAULT_STAGE = 1,
    }

    readonly string STAGE_STRING = "stage";
     private CustomLogHandler LOGGER = null;

    /// <summary>
    /// VARIABLE
    /// </summary>
    private int m_stage = (int)STAGE_ENUM.DEFAULT_STAGE;

    /// <summary>
    /// FUNCTION
    /// </summary>
	void Start ()
    {
        LOGGER = new CustomLogHandler();
        Debug.Log("test");
    }

    public void Initialize()
    {
    }

    public void Save()
    {
        PlayerPrefs.GetInt(STAGE_STRING, m_stage);
    }

    public void Load()
    {
        PlayerPrefs.SetInt(STAGE_STRING, m_stage);
    }

    
}
