using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	[ComVisible(false)]
	enum LogonSessionType : uint
	{
		Interactive = 2,
		Network,
		Batch,
		Service,
		NetworkCleartext = 8,
		NewCredentials
	}

	[ComVisible(false)]
	enum LogonProvider : uint
	{
		Default = 0, // default for platform (use this!)
		WinNT35,     // sends smoke signals to authority
		WinNT40,     // uses NTLM
		WinNT50      // negotiates Kerb or NTLM
	}

	[ComVisible(false)]
	enum SecurityLevel : uint
	{
		Anonymous = 0,
		Identification,
		Impersonation,
		Delegation
	}

	[ComVisible(false)]
	public enum SidNameUse : uint
	{
		User = 1,
		Group,
		Domain,
		Alias,
		WellKnownGroup,
		DeletedAccount,
		Invalid,
		Unknown,
		Computer,
		Label
	}

	//[ComVisible(false)]
	//public enum WellKnownSidType : uint
	//{
	//    NullSid = 0,
	//    WorldSid = 1,
	//    LocalSid = 2,
	//    CreatorOwnerSid = 3,
	//    CreatorGroupSid = 4,
	//    CreatorOwnerServerSid = 5,
	//    CreatorGroupServerSid = 6,
	//    NTAuthoritySid = 7,
	//    DialupSid = 8,
	//    NetworkSid = 9,
	//    BatchSid = 10,
	//    InteractiveSid = 11,
	//    ServiceSid = 12,
	//    AnonymousSid = 13,
	//    ProxySid = 14,
	//    EnterpriseControllersSid = 15,
	//    SelfSid = 16,
	//    AuthenticatedUserSid = 17,
	//    RestrictedCodeSid = 18,
	//    TerminalServerSid = 19,
	//    RemoteLogonIdSid = 20,
	//    LogonIdsSid = 21,
	//    LocalSystemSid = 22,
	//    LocalServiceSid = 23,
	//    NetworkServiceSid = 24,
	//    BuiltinDomainSid = 25,
	//    BuiltinAdministratorsSid = 26,
	//    BuiltinUsersSid = 27,
	//    BuiltinGuestsSid = 28,
	//    BuiltinPowerUsersSid = 29,
	//    BuiltinAccountOperatorsSid = 30,
	//    BuiltinSystemOperatorsSid = 31,
	//    BuiltinPrintOperatorsSid = 32,
	//    BuiltinBackupOperatorsSid = 33,
	//    BuiltinReplicatorSid = 34,
	//    BuiltinPreWindows2000CompatibleAccessSid = 35,
	//    BuiltinRemoteDesktopUsersSid = 36,
	//    BuiltinNetworkConfigurationOperatorsSid = 37,
	//    AccountAdministratorSid = 38,
	//    AccountGuestSid = 39,
	//    AccountKrbtgtSid = 40,
	//    AccountDomainAdminsSid = 41,
	//    AccountDomainUsersSid = 42,
	//    AccountDomainGuestsSid = 43,
	//    AccountComputersSid = 44,
	//    AccountControllersSid = 45,
	//    AccountCertAdminsSid = 46,
	//    AccountSchemaAdminsSid = 47,
	//    AccountEnterpriseAdminsSid = 48,
	//    AccountPolicyAdminsSid = 49,
	//    AccountRasAndIasServersSid = 50,
	//    NtlmAuthenticationSid = 51,
	//    DigestAuthenticationSid = 52,
	//    SChannelAuthenticationSid = 53,
	//    ThisOrganizationSid = 54,
	//    OtherOrganizationSid = 55,
	//    BuiltinIncomingForestTrustBuildersSid = 56,
	//    BuiltinPerformanceMonitoringUsersSid = 57,
	//    BuiltinPerformanceLoggingUsersSid = 58,
	//    BuiltinAuthorizationAccessSid = 59,
	//    MaxDefined = 60,
	//    BuiltinTerminalServerLicenseServersSid = 60,
	//    BuiltinDCOMUsersSid = 61,
	//    BuiltinIUsersSid = 62,
	//    IUserSid = 63,
	//    BuiltinCryptoOperatorsSid = 64,
	//    UntrustedLabelSid = 65,
	//    LowLabelSid = 66,
	//    MediumLabelSid = 67,
	//    HighLabelSid = 68,
	//    SystemLabelSid = 69,
	//    WriteRestrictedCodeSid = 70,
	//    CreatorOwnerRightsSid = 71,
	//    CacheablePrincipalsGroupSid = 72,
	//    NonCacheablePrincipalsGroupSid = 73,
	//    EnterpriseReadonlyControllersSid = 74,
	//    AccountReadonlyControllersSid = 75,
	//    BuiltinEventLogReadersGroup = 76,
	//    NewEnterpriseReadonlyControllersSid = 77,
	//    BuiltinCertSvcDComAccessGroup = 78,
	//}
}
