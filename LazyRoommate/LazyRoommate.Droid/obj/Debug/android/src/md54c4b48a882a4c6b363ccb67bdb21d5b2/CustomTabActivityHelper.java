package md54c4b48a882a4c6b363ccb67bdb21d5b2;


public class CustomTabActivityHelper
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Android.Support.CustomTabs.Chromium.SharedUtilities._MobileServices.CustomTabActivityHelper, Microsoft.Azure.Mobile.Client, Version=4.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", CustomTabActivityHelper.class, __md_methods);
	}


	public CustomTabActivityHelper () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CustomTabActivityHelper.class)
			mono.android.TypeManager.Activate ("Android.Support.CustomTabs.Chromium.SharedUtilities._MobileServices.CustomTabActivityHelper, Microsoft.Azure.Mobile.Client, Version=4.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "", this, new java.lang.Object[] {  });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
