﻿using hayward.Parsing;

namespace hayward.Tracing.Error;

public class UnimplementedMethodError(Token t, string structName, string methodName)
    : HaywardError(t, "UnimplementedMethodError", $"Struct `{structName}` has an unimplemented method `{methodName}`")
{
}