package md581ea35436e69ff6296ba639950f061de;


public class WebViewActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onOptionsItemSelected:(Landroid/view/MenuItem;)Z:GetOnOptionsItemSelected_Landroid_view_MenuItem_Handler\n" +
			"";
		mono.android.Runtime.register ("Xamarin.Auth._MobileServices.WebViewActivity, Microsoft.Azure.Mobile.Client, Version=4.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", WebViewActivity.class, __md_methods);
	}


	public WebViewActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == WebViewActivity.class)
			mono.android.TypeManager.Activate ("Xamarin.Auth._MobileServices.WebViewActivity, Microsoft.Azure.Mobile.Client, Version=4.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public boolean onOptionsItemSelected (android.view.MenuItem p0)
	{
		return n_onOptionsItemSelected (p0);
	}

	private native boolean n_onOptionsItemSelected (android.view.MenuItem p0);

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
