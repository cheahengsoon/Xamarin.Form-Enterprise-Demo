package mono.android.app;

public class ApplicationRegistration {

	public static void registerApplications ()
	{
				// Application and Instrumentation ACWs must be registered first.
		mono.android.Runtime.register ("MyEvents.Droid.MainApplication, MyEvents.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", md50d9283d3ffcc66d9a714244554f29521.MainApplication.class, md50d9283d3ffcc66d9a714244554f29521.MainApplication.__md_methods);
		
	}
}
