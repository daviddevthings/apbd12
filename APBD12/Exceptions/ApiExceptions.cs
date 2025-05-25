namespace APBD12.Exceptions;

public class TripNotFoundException : Exception
{
    public TripNotFoundException(string message = "Wycieczka o podanym ID nie istnieje.") : base(message)
    {
    }
}

public class TripAlreadyStartedException : Exception
{
    public TripAlreadyStartedException(string message = "Wycieczka już się odbyła lub jest w trakcie.") : base(message)
    {
    }
}

public class ClientAlreadyAssignedException : Exception
{
    public ClientAlreadyAssignedException(string message = "Klient jest już zapisany na tę wycieczkę.") : base(message)
    {
    }
}

public class ClientHasTripsException : Exception
{
    public ClientHasTripsException(string message = "Nie można usunąć klienta, ponieważ ma przypisane wycieczki.") : base(message)
    {
    }
}

public class ClientNotFoundException : Exception
{
    public ClientNotFoundException(string message = "Klient o podanym ID nie istnieje.") : base(message)
    {
    }
}
