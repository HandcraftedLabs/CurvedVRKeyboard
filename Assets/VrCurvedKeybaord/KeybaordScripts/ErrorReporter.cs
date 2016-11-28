using UnityEngine;
using System.Collections;

public class ErrorReporter  {

    private static ErrorReporter instance;

    private bool isErrorPresent = false;
    private bool isWariningPresent = false;
    private string currentProblemMessage = "";


    private ErrorReporter () { }


    public enum Status {
        Error,
        Warning
    }

    public static void RecreateReporter () {
        instance = new ErrorReporter();
    }

    public static ErrorReporter Instance {
        get {
            if(instance == null) {
                instance = new ErrorReporter();
            }
            return instance;
        }  
    }

    public void SetMessage(string message,Status state) {
        currentProblemMessage = message;
        if(state == Status.Error) {
            TriggerError();
        }else {
            TriggerWarning();
        }
    }

    public void Update () {
        isErrorPresent = false;
        isWariningPresent = false;
    }

    public string GetMessage () {
        return currentProblemMessage;
    }

    public bool IsErrorPresent () {
        return isErrorPresent;
    }

    public bool IsWariningPresent () {
        return isWariningPresent;
    }

    public void TriggerError ( ) {
        isErrorPresent = true;
    }
    
    public void TriggerWarning () {
        isWariningPresent = true;
    }
    
    public bool IsComunicatAviable () {
        return isErrorPresent || isWariningPresent;
    }

}
