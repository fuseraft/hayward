namespace citrus.Parsing;

public enum TokenName
{
  Default,
  Regex,
  Builtin_Argv_GetArgv,
  Builtin_Argv_GetXarg,
  Builtin_Console_Input,
  Builtin_Env_GetAll,
  Builtin_Env_GetEnvironmentVariable,
  Builtin_Env_SetEnvironmentVariable,
  Builtin_Env_UnsetEnvironmentVariable,
  Builtin_Env_Kiwi,
  Builtin_Env_KiwiLib,
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
  Builtin_Kiwi_Append,
  Builtin_Kiwi_Base,
  Builtin_Kiwi_BeginsWith,
  Builtin_Kiwi_Chars,
  Builtin_Kiwi_Clear,
  Builtin_Kiwi_Clone,
  Builtin_Kiwi_Contains,
  Builtin_Kiwi_Dequeue,
  Builtin_Kiwi_Enqueue,
  Builtin_Kiwi_Lowercase,
  Builtin_Kiwi_Empty,
  Builtin_Kiwi_EndsWith,
  Builtin_Kiwi_HasKey,
  Builtin_Kiwi_IndexOf,
  Builtin_Kiwi_IsA,
  Builtin_Kiwi_Join,
  Builtin_Kiwi_Keys,
  Builtin_Kiwi_LastIndexOf,
  Builtin_Kiwi_LeftTrim,
  Builtin_Kiwi_Lines,
  Builtin_Kiwi_Members,
  Builtin_Kiwi_Merge,
  Builtin_Kiwi_Pop,
  Builtin_Kiwi_Pretty,
  Builtin_Kiwi_Find,
  Builtin_Kiwi_Match,
  Builtin_Kiwi_Matches,
  Builtin_Kiwi_MatchesAll,
  Builtin_Kiwi_Scan,
  Builtin_Kiwi_Swap,
  Builtin_Kiwi_Set,
  Builtin_Kiwi_Get,
  Builtin_Kiwi_First,
  Builtin_Kiwi_Last,
  Builtin_Kiwi_Push,
  Builtin_Kiwi_Replace,
  Builtin_Kiwi_RReplace,
  Builtin_Kiwi_Reverse,
  Builtin_Kiwi_RightTrim,
  Builtin_Kiwi_Shift,
  Builtin_Kiwi_Unshift,
  Builtin_Kiwi_Size,
  Builtin_Kiwi_Split,
  Builtin_Kiwi_RSplit,
  Builtin_Kiwi_Substring,
  Builtin_Kiwi_ToBytes,
  Builtin_Kiwi_ToHex,
  Builtin_Kiwi_ToFloat,
  Builtin_Kiwi_ToInteger,
  Builtin_Kiwi_ToString,
  Builtin_Kiwi_ToDate,
  Builtin_Kiwi_Tokens,
  Builtin_Kiwi_Trim,
  Builtin_Kiwi_Truthy,
  Builtin_Kiwi_Type,
  Builtin_Kiwi_Uppercase,
  Builtin_Kiwi_Remove,
  Builtin_Kiwi_RemoveAt,
  Builtin_Kiwi_Rotate,
  Builtin_Kiwi_Insert,
  Builtin_Kiwi_Slice,
  Builtin_Kiwi_Concat,
  Builtin_Kiwi_Unique,
  Builtin_Kiwi_Count,
  Builtin_Kiwi_Flatten,
  Builtin_Kiwi_Values,
  Builtin_Kiwi_Zip,
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
  Builtin_Sys_EffectiveUserId,
  Builtin_Sys_Exec,
  Builtin_Sys_ExecOut,
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