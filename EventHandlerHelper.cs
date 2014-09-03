public class EventHandlerHelper<T> where T : class
{
	private BindingFlags AllBindings { get { return BindingFlags.NonPublic | BindingFlags.Instance; } }
	public int Times { get; set; }

	private Delegate GetEventHandlerDelegate(Type eventHandlerType)
	{
		var miHandler = typeof(EventHandlerHelper<T>).GetMethod("EventHandlerMock", AllBindings);
		return Delegate.CreateDelegate(eventHandlerType, this, miHandler);
	}

	public void HookUpDelegate(T classObj, string eventName)
	{
		if (classObj == null) return;
		var objType = classObj.GetType();
		var evNameInfo = objType.GetEvent(eventName);
		var delegateType = evNameInfo.EventHandlerType;
		var hookupDelegate = GetEventHandlerDelegate(delegateType);
		var addHandler = evNameInfo.GetAddMethod();
		Object[] addHandlerArgs = { hookupDelegate };
		addHandler.Invoke(classObj, addHandlerArgs); // This is will hook up the eventName with the 'EventHandlerMock' method from this class. className.eventName += EventHandlerMock
	}

	public void UnHookDelegate(T classObj, string eventName)
	{
		if (classObj == null) return;
		var objType = classObj.GetType();
		var evNameInfo = objType.GetEvent(eventName);
		var delegateType = evNameInfo.EventHandlerType;
		var unHookDelegate = GetEventHandlerDelegate(delegateType);
		evNameInfo.RemoveEventHandler(classObj, unHookDelegate); // This is will unhook the eventName with the 'EventHandlerMock' method from this class. className.eventName -= EventHandlerMock
	}

	private void EventHandlerMock(object sender, EventArgs e)
	{
		++Times;
	}
}
