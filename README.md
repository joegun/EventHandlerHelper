EventHandlerHelper
==================

This is used to un/hook event handler dynamically using Reflection in C#.
Most of the time the usage of this class is used for unit testing.

Let's take a look at the example below.

namespace yournamespace
{
  internal class yourclassname
  {
    ...
    private readonly WeakEvent<EventHandler> _onEventHandler = new WeakEvent<EventHandler>();
    public event EventHandler OnEventHandler
    {
        add { _onEventHandler.RegisterHandler(value); }
        remove { _onEventHandler.UnregisterHandler(value); }
    }
    ...
  }
}

To test that event handler, regardless what the class is that has the event handler, to make sure that the add and remove event handler working properly, you can use this class to help you out.

This is the way it works:
1. for every event handler that is hooked and called, the variable called 'Times' will get added.
2. If the event handler got unhooked, then the 'Times' will not get added.

My unit test framework is using MsTest with Typemock. So, you can use this class to help you to do something like this below.

[TestMethod]
public void OnEventHandlerAddRemoveCoverage()
{
    var eventHandlerHelper = new EventHandlerHelper<<your class name>> {Times = 0}; // Times is set as 0
    // dynamically hook up the event handler
    eventHandlerHelper.HookUpDelegate(<your class name>, "OnEventHandler");
    // Typemock trigger the event handler
    Isolate.Invoke.Event(() => <your class name>.OnEventHandler += null, string.Empty, new EventArgs());
    // Times is set to 1 now.
    Assert.AreEqual(1, eventHandlerHelper.Times);
    // dynamicaly unhook the event handler
    eventHandlerHelper.UnHookDelegate(<your class name>, "OnEventHandler");
    // Typemock try to make a call to the event handler... but nothing happens here.
    Isolate.Invoke.Event(() => <your class name>.OnEventHandler += null, string.Empty, new EventArgs());
    // Times is still set to 1
    Assert.AreEqual(1, eventHandlerHelper.Times);
}

Alright... Please let me know if you have questions.... Happy Coding and Enjoy!

