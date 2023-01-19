using System;
using System.Windows.Media;

namespace Jack.MVVM.Model
{
    public class ProgItemModel
    {
        public String Name { get; set; }

        public Guid ProgramId { get; set; }

        public ImageSource ImageSource { get; set; }

        public String[] Synonyms { get; set; }

        public String Link { get; set; }

        public String ProcessName { get; set; }
    }
}
