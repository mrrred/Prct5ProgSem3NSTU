using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XMLFramework.XMLConfigurations;

namespace XMLFramework.XMLBuilder
{
    public interface IXmlElementBuilder<T>
    {
        XElement BuildElement(T item, int id);
    }
}
