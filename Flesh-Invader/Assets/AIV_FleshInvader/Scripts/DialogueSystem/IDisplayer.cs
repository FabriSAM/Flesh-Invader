using System;

public interface IDisplayer
{

    void Open();
    void Close();
    void DisplayEntry(string text);
    Action OnEntryDisplayed {
        get;
        set;
    }

}
