using Jack.Core.Dune;
using Jack.MVVM.Model;
using Jack.MVVM.View.Windows;
using Jack.Tools.StringTLS;
using System.Threading;

namespace Jack.MVVM.ViewModel.Windows
{
    class StartupWindowViewModel : StartupWindowWorkModel
    {
        public static void StartProgram()
        {
            new MainWindow();
            var theard = new Thread(() =>
            {
                SpeechEngine.InitSpeaker();
                Thread.Sleep(2500);

                if (!ChangeTargetWindow(StartupWindow.Instance, MainWindow.Instance))
                {
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
                SpeechEngine.GiveSpeechText(StringTools.GiveRandText(AnswerDictionary.HelloAnswer), MainWindow.Instance.DuneAnswer);
            });

            theard.SetApartmentState(ApartmentState.STA);
            theard.IsBackground = true;
            theard.Start();
        }
    }
}