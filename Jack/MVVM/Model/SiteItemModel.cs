using System;
using System.Windows.Media;

namespace Jack.MVVM.Model
{
    public class SiteItemModel
    {
        public String Name { get; set; }

        public Guid SiteId { get; set; }

        public ImageSource ImageSource { get; set; }

        public String[] Synonyms { get; set; }

        public String Link { get; set; }
    }
}