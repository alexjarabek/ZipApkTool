using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sandGlass
{
    class checkItem
    {
        String m_Text = "";
        String m_Value = "";
        channelItem m_Tag = null;

        public checkItem(String Text)
        {
            m_Text = Text;
        }
        public checkItem(String Text, String Value)
        {
            m_Text = Text;
            m_Value = Value;         
        }
        public checkItem(String Text, String Value, channelItem Tag)
        {
            m_Text = Text;
            m_Value = Value;
            m_Tag = Tag;
        }
        public channelItem Tag
        {
            get { return m_Tag; }
            set { m_Tag = value; }
        }
        public String Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }
        public String Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public override string ToString()
        {
            return this.Text+"-"+this.Value;
        }
    }
}
