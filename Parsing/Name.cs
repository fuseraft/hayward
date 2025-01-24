namespace citrus.Parsing;

public enum TokenName
{
  Default,
  Regex,
  Builtin_Argv_GetArgv,
  Builtin_Argv_GetXarg,
  Builtin_Console_Input,
  Builtin_Console_Foreground,
  Builtin_Console_Background,
  Builtin_Console_Reset,
  Builtin_Console_Clear,
  Builtin_Env_OS,
  Builtin_Env_User,
  Builtin_Env_UserDomain,
  Builtin_Env_GetAll,
  Builtin_Env_GetEnvironmentVariable,
  Builtin_Env_SetEnvironmentVariable,
  Builtin_Env_Citrus,
  Builtin_Env_CitrusLib,
  Builtin_FFI_Attach,
  Builtin_FFI_Invoke,
  Builtin_FFI_Load,
  Builtin_FFI_Unload,
  Builtin_FileIO_AppendText,
  Builtin_FileIO_ChangeDirectory,
  Builtin_FileIO_CopyFile,
  Builtin_FileIO_CopyR,
  Builtin_FileIO_Combine,
  Builtin_FileIO_CreateFile,
  Builtin_FileIO_DeleteFile,
  Builtin_FileIO_FileExists,
  Builtin_FileIO_FileName,
  Builtin_FileIO_FileSize,
  Builtin_FileIO_GetCurrentDirectory,
  Builtin_FileIO_GetFileAbsolutePath,
  Builtin_FileIO_GetFileAttributes,
  Builtin_FileIO_GetFileExtension,
  Builtin_FileIO_GetFilePath,
  Builtin_FileIO_Glob,
  Builtin_FileIO_IsDirectory,
  Builtin_FileIO_ListDirectory,
  Builtin_FileIO_MakeDirectory,
  Builtin_FileIO_MakeDirectoryP,
  Builtin_FileIO_MoveFile,
  Builtin_FileIO_ReadBytes,
  Builtin_FileIO_ReadFile,
  Builtin_FileIO_ReadLines,
  Builtin_FileIO_RemoveDirectory,
  Builtin_FileIO_RemoveDirectoryF,
  Builtin_FileIO_TempDir,
  Builtin_FileIO_WriteBytes,
  Builtin_FileIO_WriteLine,
  Builtin_FileIO_WriteText,
  Builtin_Citrus_Append,
  Builtin_Citrus_Base,
  Builtin_Citrus_BeginsWith,
  Builtin_Citrus_Chars,
  Builtin_Citrus_Clear,
  Builtin_Citrus_Clone,
  Builtin_Citrus_Contains,
  Builtin_Citrus_Dequeue,
  Builtin_Citrus_Enqueue,
  Builtin_Citrus_Lowercase,
  Builtin_Citrus_Empty,
  Builtin_Citrus_EndsWith,
  Builtin_Citrus_HasKey,
  Builtin_Citrus_IndexOf,
  Builtin_Citrus_IsA,
  Builtin_Citrus_Join,
  Builtin_Citrus_Keys,
  Builtin_Citrus_LastIndexOf,
  Builtin_Citrus_LeftTrim,
  Builtin_Citrus_Lines,
  Builtin_Citrus_Members,
  Builtin_Citrus_Merge,
  Builtin_Citrus_Pop,
  Builtin_Citrus_Pretty,
  Builtin_Citrus_Find,
  Builtin_Citrus_Match,
  Builtin_Citrus_Matches,
  Builtin_Citrus_MatchesAll,
  Builtin_Citrus_Scan,
  Builtin_Citrus_Swap,
  Builtin_Citrus_Set,
  Builtin_Citrus_Get,
  Builtin_Citrus_First,
  Builtin_Citrus_Last,
  Builtin_Citrus_Push,
  Builtin_Citrus_Replace,
  Builtin_Citrus_RReplace,
  Builtin_Citrus_Reverse,
  Builtin_Citrus_RightTrim,
  Builtin_Citrus_Shift,
  Builtin_Citrus_Unshift,
  Builtin_Citrus_Size,
  Builtin_Citrus_Split,
  Builtin_Citrus_RSplit,
  Builtin_Citrus_Substring,
  Builtin_Citrus_ToBytes,
  Builtin_Citrus_ToHex,
  Builtin_Citrus_ToFloat,
  Builtin_Citrus_ToInteger,
  Builtin_Citrus_ToString,
  Builtin_Citrus_ToDate,
  Builtin_Citrus_Tokens,
  Builtin_Citrus_Trim,
  Builtin_Citrus_Truthy,
  Builtin_Citrus_Type,
  Builtin_Citrus_Uppercase,
  Builtin_Citrus_Remove,
  Builtin_Citrus_RemoveAt,
  Builtin_Citrus_Rotate,
  Builtin_Citrus_Insert,
  Builtin_Citrus_Slice,
  Builtin_Citrus_Concat,
  Builtin_Citrus_Unique,
  Builtin_Citrus_Count,
  Builtin_Citrus_Flatten,
  Builtin_Citrus_Values,
  Builtin_Citrus_Zip,
  Builtin_Encoder_Base64Encode,
  Builtin_Encoder_Base64Decode,
  Builtin_Encoder_UrlEncode,
  Builtin_Encoder_UrlDecode,
  Builtin_List_All,
  Builtin_List_Each,
  Builtin_List_Map,
  Builtin_List_Max,
  Builtin_List_Min,
  Builtin_List_None,
  Builtin_List_Reduce,
  Builtin_List_Select,
  Builtin_List_Sort,
  Builtin_List_Sum,
  Builtin_List_ToH,
  Builtin_Logging_FilePath,
  Builtin_Logging_Mode,
  Builtin_Logging_EntryFormat,
  Builtin_Logging_TimestampFormat,
  Builtin_Logging_Level,
  Builtin_Logging_Debug,
  Builtin_Logging_Warn,
  Builtin_Logging_Info,
  Builtin_Logging_Error,
  Builtin_Signal_Send,
  Builtin_Signal_Raise,
  Builtin_Signal_Trap,
  Builtin_Socket_Create,
  Builtin_Socket_Bind,
  Builtin_Socket_Listen,
  Builtin_Socket_Accept,
  Builtin_Socket_Connect,
  Builtin_Socket_Send,
  Builtin_Socket_SendRaw,
  Builtin_Socket_Receive,
  Builtin_Socket_Close,
  Builtin_Socket_Shutdown,
  Builtin_Net_ResolveHost,
  Builtin_Net_IsIPAddr,
  Builtin_WebClient_Delete,
  Builtin_WebClient_Get,
  Builtin_WebClient_Head,
  Builtin_WebClient_Options,
  Builtin_WebClient_Patch,
  Builtin_WebClient_Post,
  Builtin_WebClient_Put,
  Builtin_WebServer_Get,
  Builtin_WebServer_Post,
  Builtin_WebServer_Listen,
  Builtin_WebServer_Host,
  Builtin_WebServer_Port,
  Builtin_WebServer_Public,
  Builtin_Math_Abs,
  Builtin_Math_Acos,
  Builtin_Math_Asin,
  Builtin_Math_Atan,
  Builtin_Math_Atan2,
  Builtin_Math_Cbrt,
  Builtin_Math_Ceil,
  Builtin_Math_CopySign,
  Builtin_Math_Cos,
  Builtin_Math_Cosh,
  Builtin_Math_Divisors,
  Builtin_Math_Epsilon,
  Builtin_Math_Erf,
  Builtin_Math_ErfC,
  Builtin_Math_Exp,
  Builtin_Math_ExpM1,
  Builtin_Math_FDim,
  Builtin_Math_Floor,
  Builtin_Math_FMax,
  Builtin_Math_FMin,
  Builtin_Math_Fmod,
  Builtin_Math_Hypot,
  Builtin_Math_IsFinite,
  Builtin_Math_IsInf,
  Builtin_Math_IsNaN,
  Builtin_Math_IsNormal,
  Builtin_Math_LGamma,
  Builtin_Math_Log,
  Builtin_Math_Log10,
  Builtin_Math_Log1P,
  Builtin_Math_Log2,
  Builtin_Math_NextAfter,
  Builtin_Math_Pow,
  Builtin_Math_Random,
  Builtin_Math_Remainder,
  Builtin_Math_Round,
  Builtin_Math_RotateLeft,
  Builtin_Math_RotateRight,
  Builtin_Math_Sin,
  Builtin_Math_Sinh,
  Builtin_Math_Sqrt,
  Builtin_Math_Tan,
  Builtin_Math_Tanh,
  Builtin_Math_TGamma,
  Builtin_Math_Trunc,
  Builtin_Math_ListPrimes,
  Builtin_Math_NthPrime,
  Builtin_Package_Home,
  Builtin_Reflector_RBin,
  Builtin_Reflector_RFFlags,
  Builtin_Reflector_RInspect,
  Builtin_Reflector_RLib,
  Builtin_Reflector_RList,
  Builtin_Reflector_RObject,
  Builtin_Reflector_RRetVal,
  Builtin_Reflector_RStack,
  Builtin_Serializer_Serialize,
  Builtin_Serializer_Deserialize,
  Builtin_Sys_Exec,
  Builtin_Sys_ExecOut,
  Builtin_Sys_Open,
  Builtin_Task_Busy,
  Builtin_Task_List,
  Builtin_Task_Result,
  Builtin_Task_Sleep,
  Builtin_Task_Status,
  Builtin_Time_AMPM,
  Builtin_Time_Millisecond,
  Builtin_Time_Hour,
  Builtin_Time_IsDST,
  Builtin_Time_Minute,
  Builtin_Time_Month,
  Builtin_Time_MonthDay,
  Builtin_Time_Second,
  Builtin_Time_Ticks,
  Builtin_Time_TicksToMilliseconds,
  Builtin_Time_Now,
  Builtin_Time_WeekDay,
  Builtin_Time_Year,
  Builtin_Time_YearDay,
  Builtin_Time_AddDay,
  Builtin_Time_AddMonth,
  Builtin_Time_AddYear,
  Builtin_Time_AddHour,
  Builtin_Time_AddMinute,
  Builtin_Time_AddSecond,
  Builtin_Time_AddMillisecond,
  KW_Abstract,
  KW_As,
  KW_Async,
  KW_Await,
  KW_Break,
  KW_Case,
  KW_Catch,
  KW_Const,
  KW_Do,
  KW_Else,
  KW_ElseIf,
  KW_End,
  KW_EPrint,
  KW_EPrintLn,
  KW_Exit,
  KW_Export,
  KW_False,
  KW_Finally,
  KW_For,
  KW_Spawn,
  KW_If,
  KW_Import,
  KW_In,
  KW_Interface,
  KW_Lambda,
  KW_Method,
  KW_Package,
  KW_Next,
  KW_Null,
  KW_Override,
  KW_Parse,
  KW_Pass,
  KW_Print,
  KW_PrintLn,
  KW_PrintXy,
  KW_Private,
  KW_Repeat,
  KW_Return,
  KW_Static,
  KW_Struct,
  KW_This,
  KW_Throw,
  KW_True,
  KW_Try,
  KW_Var,
  KW_When,
  KW_While,
  Ops_Add,
  Ops_AddAssign,
  Ops_And,
  Ops_AndAssign,
  Ops_Assign,
  Ops_BitwiseAnd,
  Ops_BitwiseAndAssign,
  Ops_BitwiseLeftShift,
  Ops_BitwiseLeftShiftAssign,
  Ops_BitwiseNot,
  Ops_BitwiseNotAssign,
  Ops_BitwiseOr,
  Ops_BitwiseOrAssign,
  Ops_BitwiseRightShift,
  Ops_BitwiseRightShiftAssign,
  Ops_BitwiseUnsignedRightShift,
  Ops_BitwiseUnsignedRightShiftAssign,
  Ops_BitwiseXor,
  Ops_BitwiseXorAssign,
  Ops_Divide,
  Ops_DivideAssign,
  Ops_Equal,
  Ops_Exponent,
  Ops_ExponentAssign,
  Ops_GreaterThan,
  Ops_GreaterThanOrEqual,
  Ops_LessThan,
  Ops_LessThanOrEqual,
  Ops_ModuloAssign,
  Ops_Modulus,
  Ops_Multiply,
  Ops_MultiplyAssign,
  Ops_Not,
  Ops_NotEqual,
  Ops_Or,
  Ops_OrAssign,
  Ops_Subtract,
  Ops_SubtractAssign,
  Types_Any,
  Types_Boolean,
  Types_Float,
  Types_Hashmap,
  Types_Integer,
  Types_Date,
  Types_Lambda,
  Types_List,
  Types_None,
  Types_Object,
  Types_String,
  Types_Pointer
}