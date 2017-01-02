﻿
using UnityEngine;

namespace CurvedVRKeyboard {


    public class ErrorReporter {

        private static ErrorReporter instance;
        private string currentProblemMessage = "";

        public Status currentStatus = Status.None;
        public enum Status {
            Error, Warning, Info, None
        }


        private ErrorReporter () { }

        public static ErrorReporter Instance {
            get {
                if(instance == null) {
                    instance = new ErrorReporter();
                }
                return instance;
            }
        }

        public void SetMessage ( string message, Status state ) {
            currentProblemMessage = message;
            if(state == Status.Error) {
                TriggerError();
            } else if(state == Status.Warning){
                TriggerWarning();
            }else if (state == Status.Info) {
                TriggerInfo();
            }
        }

        public void Reset () {
            currentStatus = Status.None;
        }

        public string GetMessage () {
            return currentProblemMessage;
        }

        public bool IsErrorPresent () {
            return currentStatus == Status.Error;
        }

        public bool IsWarningPresent () {
            return currentStatus == Status.Warning;
        }

        public bool IsInfoPresent () {
            return currentStatus == Status.Info;
        }

        public void TriggerError () {
            currentStatus = Status.Error;
        }

        public void TriggerWarning () {
            currentStatus = Status.Warning;
        }
        
        public void TriggerInfo () {
            currentStatus = Status.Info;
        }

        public bool ShouldMessageBeDisplayed () {
            return currentStatus != Status.None;
        }

        public Color GetMessageColor () {
            if(IsErrorPresent()) {
                return Color.red;
            }else if(IsWarningPresent()) {
                return Color.yellow;
            }else {
                return Color.cyan;
            }

        }
    }
}