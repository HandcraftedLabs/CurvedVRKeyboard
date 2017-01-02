
namespace CurvedVRKeyboard {


    public class ErrorReporter {

        private static ErrorReporter instance;

        //----Comunication-----
        private bool isErrorPresent = false;
        private bool isWarningPresent = false;
        private bool isInfoPresent = false;
        private string currentProblemMessage = "";

        public enum Status {
            Error, Warning, Info
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

            }
        }

        public void Reset () {
            isErrorPresent = false;
            isWarningPresent = false;
            isInfoPresent = false;
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

        public void TriggerError () {
            isErrorPresent = true;
        }

        public void TriggerWarning () {
            isWarningPresent = true;
        }
        
        public void TriggerInfo () {
            isInfoPresent = true;
        }

        public bool IsInfoPresent () {
            return isInfoPresent;
        }

        public bool ShouldMessageBeDisplayed () {
            return isErrorPresent || isWarningPresent;
        }
    }
}