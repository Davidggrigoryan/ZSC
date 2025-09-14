#if !ZSC_USE_FIREBASE
namespace Firebase { public class AppOptions {} public class DependencyStatus {} public class FirebaseApp {} }
namespace Firebase.Database { public class FirebaseDatabase {} }
namespace Firebase.Auth { public class FirebaseAuth {} public class FirebaseUser {} }
namespace Firebase.DynamicLinks {
  public class DynamicLinkComponents {}
  public class ReceivedDynamicLinkEventArgs : System.EventArgs {}
}
#endif
