using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Qrame.CoreFX.ExpandObjects.DataObject
{
    /*
    dynamic person = DynamicXml.Parse("<Person><Name>Matt</Name><Age>28</Age><IsAwesome>true</IsAwesome></Person>");
    string name = person.Name; // Matt
    int age = person.Age; // 28
    bool isAwesome = person.IsAwesome; // true
    */
    /// <summary>
    /// Allows XML data to be accessed as a dynamic object.
    /// </summary>
    public class DynamicXml : DynamicObject
    {
        /// <summary>
        /// Returns a new DynamicXml instance created by parsing the specified XML string.
        /// </summary>
        /// <param name="text">A string that contains well-formed XML.</param>
        /// <returns>A new DynamicXml instance.</returns>
        public static DynamicXml Parse(string text)
        {
            return new DynamicXml(XElement.Parse(text));
        }

        /// <summary>
        /// The XML element represented by this class.
        /// </summary>
        private readonly XElement root;

        /// <summary>
        /// Initializes a new instance of the DynamicXml class.
        /// </summary>
        /// <param name="root">The XML element to be represented.</param>
        private DynamicXml(XElement root)
        {
            this.root = root;
        }

        /// <summary>
        /// Provides the implementation for operations that get member values.
        /// </summary>
        /// <param name="binder">
        /// Provides information about the object that called the dynamic operation.
        /// </param>
        /// <param name="result">
        /// The result of the get operation.
        /// </param>
        /// <returns>
        /// true if the operation is successful; otherwise, false.
        /// </returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            XElement[] nodes = this.root.Elements(binder.Name).ToArray();
            if (nodes.Length > 1)
            {
                result = nodes.Select(o => new DynamicXml(o)).ToArray();
                return true;
            }
            else if (nodes.Length == 1)
            {
                result = new DynamicXml(nodes.First());
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Provides implementation for type conversion operations
        /// </summary>
        /// <param name="binder">
        /// Provides information about the conversion operation.
        /// </param>
        /// <param name="result">
        /// The result of the type conversion operation.
        /// </param>
        /// <returns></returns>
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            try
            {
                result = Convert.ChangeType(this.root.Value, binder.Type);
                return true;
            }
            catch (Exception exception)
            {
                if (exception is InvalidCastException ||
                    exception is FormatException ||
                    exception is OverflowException ||
                    exception is ArgumentNullException)
                {
                    result = null;
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
