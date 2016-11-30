using UnityEngine;
using System.Collections;

public class ErrorReporter  {

    private static ErrorReporter instance;

    private bool isErrorPresent = false;
    private bool isWarningPresent = false;
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

    public void Reset () {
        isErrorPresent = false;
        isWarningPresent = false;
    }

    public string GetMessage () {
        return currentProblemMessage;
    }

    public bool IsErrorPresent () {
        return isErrorPresent;
    }

    public bool IsWarningPresent () {
        return isWarningPresent;
    }

    public void TriggerError ( ) {
        isErrorPresent = true;
    }
    
    public void TriggerWarning () {
        isWarningPresent = true;
    }
    
    public bool ShouldMessageBeDisplayed () {
        return isErrorPresent || isWarningPresent;
    }

}
