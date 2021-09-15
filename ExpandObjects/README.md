﻿# Expando Class
#### Extensible dynamic types for .NET that support both static and dynamic properties

Expando is a .NET class that allows you to create extensible types that mix the functionality of static and dynamic types. You can create static types inherited from Expando that have all the features of the static type, but when cast to dynamic support extensibility via dynamic C# features that allow you to add properties and methods at runtime. You can also create mix-ins that combine the properties from two objects into a single object.

The library supports two-way serialization with JSON.NET and XmlSerializer which allows building extensible types that can persist and restore themselves. This is very useful for data models that can expose extensible custom properties that can be persisted as serialized strings for example.

This class provides functionality similar to the native `ExpandoObject` class, but with many more features for working with existing statically typed Types, the ability to serialize and to inherit from to extend existing types.

### Features

Expando has the following features:

* Allows strongly typed classes (must inherit from Expando)
* Supports strongly typed Properties and Methods
* Supports dynamically added Properties and Methods
* Allows extension of strongly typed classes with dynamic features
* Supports string based collection access to properties<br/>
(both on static and dynamic properties)
* Create Mix-ins of two types that combine properties
* Supports two-way JSON.NET and XML serialization

You can find out more detail from this blog post:
[Creating a dynamic, extensible C# Expando Object](http://www.west-wind.com/weblog/posts/2012/Feb/08/Creating-a-dynamic-extensible-C-Expando-Object)

### Installation
You can use this class with source code provided here or by using the [Westwind.Utilities NuGet package](https://www.nuget.org/packages/Westwind.Utilities).

```
pm> Install-Package Westwind.Utilities
```

### Example Usage
This class essentially acts as a mix-in where you can create a strongly typed object and add dynamic properties to it.

To start create a class that inherits from the Expando class and simply create your class as usual by adding properties:

```c#
public class User : Westwind.Utilities.Dynamic.Expando
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public bool Active { get; set; }
    public DateTime? ExpiresOn { get; set; }

    public User() : base()
    { }

    // only required if you want to mix in seperate instance
    public User(object instance)
        : base(instance)
    {
    }
}
```

Then simply instantiate the class. If you reference the strongly typed class properties you get the strongly typed interface - ie. your declared properties:

```c#
var user = new User();
user.Email = "rick@whatsa.com"
```

If you cast the object to dynamic you can also attach and read any new properties.

```c#
dynamic duser = user;
duser.WhatsUp = "Hella"
duser["WhatTime"] = DateTime.Now;

string wu = duser.WhatsUp;
string wt = duser["WhatTime"];
wu = duser.["WhatsUp"]; // also works
wt = duser.WhatTime;  // also works
```

The following sequence demonstrates in more detail:

```c#
var user = new User();

// Set strongly typed properties
user.Email = "rick@west-wind.com";
user.Password = "nonya123";
user.Name = "Rickochet";
user.Active = true;

// Now add dynamic properties
dynamic duser = user;
duser.Entered = DateTime.Now;
duser.Accesses = 1;

// you can also add dynamic props via indexer 
user["NickName"] = "AntiSocialX";
duser["WebSite"] = "http://www.west-wind.com/weblog";
        
// Access strong type through dynamic ref
Assert.AreEqual(user.Name,duser.Name);

// Access strong type through indexer 
Assert.AreEqual(user.Password,user["Password"]);
        

// access dyanmically added value through indexer
Assert.AreEqual(duser.Entered,user["Entered"]);
        
// access index added value through dynamic
Assert.AreEqual(user["NickName"],duser.NickName);
        

// loop through all properties dynamic AND strong type properties (true)
foreach (var prop in user.GetProperties(true))
{ 
    object val = prop.Value;
    if (val == null)
        val = "null";

    Console.WriteLine(prop.Key + ": " + val.ToString());
}
```
### Serialization
The Expando class supports JSON.NET and XmlSerializer two-way serialization. So you can create objects that contain combined static and dynamic properties and have both persist and restore to and from serialized content.

```c#
// Set standard properties
var ex = new User()
{
    Name = "Rick",
    Email = "rstrahl@whatsa.com",
    Password = "Seekrit23",
    Active = true
};

// set dynamic properties
dynamic exd = ex;
exd.Entered = DateTime.Now;
exd.Company = "West Wind";
exd.Accesses = 10;

// set dynamic properties as dictionary
ex["Address"] = "32 Kaiea";
ex["Email"] = "rick@west-wind.com";
ex["TotalOrderAmounts"] = 51233.99M;

// *** Should serialize both static properties dynamic properties
var json = JsonConvert.SerializeObject(ex, Formatting.Indented);
Console.WriteLine("*** Serialized Native object:");
Console.WriteLine(json);

Assert.IsTrue(json.Contains("Name")); // static
Assert.IsTrue(json.Contains("Company")); // dynamic


// *** Now deserialize the JSON back into object to 
// *** check for two-way serialization
var user2 = JsonConvert.DeserializeObject<User>(json);
json = JsonConvert.SerializeObject(user2, Formatting.Indented);
Console.WriteLine("*** De-Serialized User object:");
Console.WriteLine(json);

Assert.IsTrue(json.Contains("Name")); // static
Assert.IsTrue(json.Contains("Company")); // dynamic
```

This produces the following JSON that mixes both the static and dynamic properties as a single JSON object literal:

```json
{
  "Email": "rstrahl@whatsa.com",
  "Password": "Seekrit23",
  "Name": "Rick",
  "Active": true,
  "ExpiresOn": null,
  "Entered": "2015-01-28T11:22:18.3548271-10:00",
  "Company": "West Wind",
  "Accesses": 10,
  "Address": "32 Kaiea",
  "Email": "rstrahl@whatsa.com",
  "TotalOrderAmounts": 51233.99
}
```  

The same code using XML Serialization:

```c#
// Set standard properties
var ex = new User();
ex.Name = "Rick";
ex.Active = true;


// set dynamic properties
dynamic exd = ex;
exd.Entered = DateTime.Now;
exd.Company = "West Wind";
exd.Accesses = 10;

// set dynamic properties as dictionary
ex["Address"] = "32 Kaiea";
ex["Email"] = "rick@west-wind.com";
ex["TotalOrderAmounts"] = 51233.99M;

// Serialize creates both static and dynamic properties
// dynamic properties are serialized as a 'collection'
string xml;
SerializationUtils.SerializeObject(exd, out xml);
Console.WriteLine("*** Serialized Dynamic object:");
Console.WriteLine(xml);

Assert.IsTrue(xml.Contains("Name")); // static
Assert.IsTrue(xml.Contains("Company")); // dynamic

// Serialize
var user2 = SerializationUtils.DeSerializeObject(xml,typeof(User));
SerializationUtils.SerializeObject(exd, out xml);
Console.WriteLine(xml);

Assert.IsTrue(xml.Contains("Rick")); // static
Assert.IsTrue(xml.Contains("West Wind")); // dynamic
```

Produces the following XML:

```xml
<?xml version="1.0" encoding="utf-8"?>
<User xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
   <Properties>
      <item>
         <key>Entered</key>
         <value type="datetime">2015-01-28T11:24:27.0119386-10:00</value>
      </item>
      <item>
         <key>Company</key>
         <value>West Wind</value>
      </item>
      <item>
         <key>Accesses</key>
         <value type="integer">10</value>
      </item>
      <item>
         <key>Address</key>
         <value>32 Kaiea</value>
      </item>
      <item>
         <key>Email</key>
         <value>rick@west-wind.com</value>
      </item>
      <item>
         <key>TotalOrderAmounts</key>
         <value type="decimal">51233.99</value>
      </item>
   </Properties>
   <Name>Rick</Name>
   <Active>true</Active>
   <ExpiresOn xsi:nil="true" />
</User>
```

Note that the XML serializes the dynaimc properties as a collection courtesy of the PropertyBag() custom XML serializer. Although this XML schema isn't as clean as the JSON, it does work with two-way serialization to properly deserialize the object.

### Downsides
There are a few issues you should be aware of when you use this class. They're not show stoppers by any means, but keep the following in mind:

##### Must inherit
First, you have to inherit from Expando in order to use it to extend an existing type. So you'll always introduce an extra layer of inheritance. For many use cases this isn't a problem, but if you need to inherit multiple levels this can become a problem. A work around for this is to use composition by adding an Expando object property to an existing object to provide the dynamic extensibility and mix-in features to your type.

##### Noisy Class Interface
This class inherits from DynamicObject and implements the required methods on that class to provide the dynamic features. These methods end up on the class interface you inherit. Not clean, but luckily most base methods group together (TryXXXX methods).

##### Performance
If you stick purely to the static interface of the class performance should be excellent. But once you start accessing the dynamic properties you're into late runtime binding (or at least one-time late binding) and there will be some performance hit for using dynamic properties. However, dynamic can actually be very fast after the first access of each member. It's a tradeoff for the flexibility you get. Make sure you use a static reference for the static interface where possible and explicitly use the dynamic interface when accessing the dynamic properties. 

## License
This Expando library is an open source and licensed under **[MIT license](http://opensource.org/licenses/MIT)**, and there's no charge to use, integrate or modify the code for this project. You are free to use it in personal, commercial, government and any other type of application. Commercial licenses are also available.

All source code is copyright West Wind Technologies, regardless of changes made to them. Any source code modifications must leave the original copyright code headers intact.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

copyright, West Wind Technologies, 2012 - 2015

