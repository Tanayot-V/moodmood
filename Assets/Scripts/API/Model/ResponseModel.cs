using System.Collections.Generic;

[System.Serializable]
public class AccountResponseModel
{
    public int student_id;
    public string student_number;
    public string email;
    public string sex;
    public string prefix_name;
    public string first_name;
    public string last_name;
    public int school_id;
    public int room_id;
    public string school_name;
    public string room_name;
}

[System.Serializable]
public class MoodData
{
    public int id;
    public string student_id;
    public string date;
    public string time;
    public string mood;
    public string mood_choice;
    public string mood_choice_txt;
    public string note;
    public Dictionary<string,Trigger> triggers;
}

[System.Serializable]
public class Trigger
{
    public string trigger;
    public string trigger_txt;
    public Dictionary<string,Handle> handles;
}

[System.Serializable]
public class Handle
{
    public string handle;
    public HandleChoices[] handle_choices;
}

[System.Serializable]
public class HandleChoices
{
    public string handle_choice;
    public string handle_txt;
}

[System.Serializable]
public class CreateMoodRequest
{
    public CreateMoodData data;
}

[System.Serializable]
public class CreateMoodData
{
    public int student_id;
    public int mood;
    public int mood_choice;
    public CreateTriggerRequest[] triggers;
    public string note;
    public string happy_note;
}
[System.Serializable]
public class CreateTriggerRequest
{
    public int trigger;
    public CreateHandleRequest[] handles;

}

[System.Serializable]
public class CreateHandleRequest
{
    public int handle;
    public int[] handle_choices;
}   


[System.Serializable]
public class GetOveraLLResponse
{
    public MoodsSum moods_sum;
    public int total;
}

[System.Serializable]
public class MoodsSum
{
    public int m_0;
    public int m_1;
    public int m_2;
    public int m_3;
}

[System.Serializable]
public class GetOveraLLDetailResponse
{
    public Dictionary<string, MoodChoiceData> mood_choices;
    public Dictionary<string, TriggerChoicesData> trigger_choices;
    public Dictionary<string, Handle> handles;
    public NoteData[] notes;
}

[System.Serializable]
public class MoodChoiceData
{
    public string mood_choice;
    public string mood_choice_txt;
}
[System.Serializable]
public class TriggerChoicesData
{
    public string trigger;
    public string trigger_txt;
}

[System.Serializable]
public class NoteData
{
    public int id;
    public string date;
    public string time;
    public string note;
    public string happy_note;
}