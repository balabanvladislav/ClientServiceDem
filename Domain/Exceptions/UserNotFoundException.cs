using System;

namespace Domain.Exceptions;

public class UserNotFoundException(string ldapUsername) : Exception($"User {ldapUsername} not found, or incorrect password");