package md5214eafb7e7b3b7fcc363a68a6358563f;


public abstract class GcmServiceBase
	extends mono.android.app.IntentService
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onHandleIntent:(Landroid/content/Intent;)V:GetOnHandleIntent_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("Gcm.Client.GcmServiceBase, GCM.Client, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", GcmServiceBase.class, __md_methods);
	}


	public GcmServiceBase (java.lang.String p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == GcmServiceBase.class)
			mono.android.TypeManager.Activate ("Gcm.Client.GcmServiceBase, GCM.Client, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}


	public GcmServiceBase () throws java.lang.Throwable
	{
		super ();
		if (getClass () == GcmServiceBase.class)
			mono.android.TypeManager.Activate ("Gcm.Client.GcmServiceBase, GCM.Client, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public GcmServiceBase (java.lang.String[] p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == GcmServiceBase.class)
			mono.android.TypeManager.Activate ("Gcm.Client.GcmServiceBase, GCM.Client, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "System.String[], mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}


	public void onHandleIntent (android.content.Intent p0)
	{
		n_onHandleIntent (p0);
	}

	private native void n_onHandleIntent (android.content.Intent p0);

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
