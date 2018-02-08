package md5214eafb7e7b3b7fcc363a68a6358563f;


public abstract class GcmBroadcastReceiverBase_1
	extends android.content.BroadcastReceiver
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onReceive:(Landroid/content/Context;Landroid/content/Intent;)V:GetOnReceive_Landroid_content_Context_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("Gcm.Client.GcmBroadcastReceiverBase`1, GCM.Client, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", GcmBroadcastReceiverBase_1.class, __md_methods);
	}


	public GcmBroadcastReceiverBase_1 ()
	{
		super ();
		if (getClass () == GcmBroadcastReceiverBase_1.class)
			mono.android.TypeManager.Activate ("Gcm.Client.GcmBroadcastReceiverBase`1, GCM.Client, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onReceive (android.content.Context p0, android.content.Intent p1)
	{
		n_onReceive (p0, p1);
	}

	private native void n_onReceive (android.content.Context p0, android.content.Intent p1);

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
