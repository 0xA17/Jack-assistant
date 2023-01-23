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

                if (!ChangeTargetWindow(StartupWindow.Instance, MainWindow.GetInstance()))
                {
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
                SpeechEngine.GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.HelloAnswer), MainWindow.GetInstance().DuneAnswer);
            });

            theard.SetApartmentState(ApartmentState.STA);
            theard.IsBackground = true;
            theard.Start();
        }
    }
}