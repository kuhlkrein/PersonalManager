using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SaveSerial : MonoBehaviour
{
    public int _index_courant = 0;
    public List<HistoriqueDeTache> _historique;
    public List<Tache> _taches_a_faire;

    private void Start()
    {
        LoadGame();
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
                     + "/mainSettings.dat");
        Sauvegarde data = new Sauvegarde();
        data.taches_a_faire = _taches_a_faire;
        data.historique = _historique;
        data.index_courant = _index_courant;
        bf.Serialize(file, data);
        file.Close();
        Debug.LogWarning("Game data saved!");
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath
                       + "/mainSettings.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
                       File.Open(Application.persistentDataPath
                       + "/mainSettings.dat", FileMode.Open);
            Sauvegarde data = (Sauvegarde)bf.Deserialize(file);
            file.Close();
            _taches_a_faire = data.taches_a_faire;

            foreach (Tache _task in _taches_a_faire)
            {
                foreach (Session session in _task.liste_de_sessions)
                {
                    int iteration = 0;
                    while (session.horaire.Date.DayOfYear < DateTime.Today.DayOfYear && iteration < 365)
                    {
                        session.horaire = session.horaire.Date.AddDays(_task.daySpace);
                        iteration++;
                    }
                }

            }
            _historique = data.historique;
            _index_courant = data.index_courant;
            Debug.LogWarning("Game data loaded!");
            SaveGame();
        }
        else
        {
            Debug.LogError("There is no save data! Reseting.");
            ResetData();
        }
    }

    void ResetData()
    {
        _taches_a_faire = new List<Tache>();
        _index_courant = 0;
        _historique = new List<HistoriqueDeTache>();
        if (File.Exists(Application.persistentDataPath + "/mainSettings.dat"))
        {
            File.Delete(Application.persistentDataPath + "/mainSettings.dat");
            Debug.LogWarning("Data reset complete!");
        }
        else
        {
            Debug.LogError("No save data to delete, new data created!");
        }
        SaveGame();
    }
}























// CLASSES UTILES


[Serializable]
class Sauvegarde
{
    public int index_courant = 0;
    public List<HistoriqueDeTache> historique = new List<HistoriqueDeTache>();
    public List<Tache> taches_a_faire = new List<Tache>();
}

[Serializable]
public class HistoriqueDeTache
{
    public int id = -1;
    public List<ObjectifJournalier> objectifsJournaliers = new List<ObjectifJournalier>();
    public HistoriqueDeTache(int taskId)
    {
        id = taskId;
    }

    public void validateObjective(Tache task)
    {
        ObjectifJournalier last;
        if (objectifsJournaliers.Count != 0)
        {
            last = objectifsJournaliers.Last<ObjectifJournalier>();
            if (last.jour == DateTime.Today)
            {
                last.fait += 1;
                return;
            }
        }
        objectifsJournaliers.Add(new ObjectifJournalier(task));

    }
}

[Serializable]
public class Tache
{
    public int id = -1;
    public String description = "description";
    public float valeur_actuelle = 1;
    public float modificateur_d_amelioration = 1;
    public String type_de_modificateur = "*%"; // {* || +}{%}
    public List<Session> liste_de_sessions = new List<Session>();
    public int daySpace = 1;
    public int combo = 0;

    public Tache(String descr, float initialValue, float modificateur, string modifType, DateTime horaire_A_Repeter, int dayBetweenTask = 1)
    {
        description = descr;
        valeur_actuelle = initialValue;
        modificateur_d_amelioration = modificateur;
        type_de_modificateur = modifType;
        liste_de_sessions.Add(new Session(horaire_A_Repeter));
        daySpace = dayBetweenTask;
    }

    public void addSession(DateTime horaire_A_Repeter)
    {
        liste_de_sessions.Add(new Session(horaire_A_Repeter));
    }

    public bool updateSessions(List<Session> sessionList)
    {
        Debug.Log("new Sessions");
        foreach (Session newSess in sessionList)
        {
            Debug.Log(newSess.horaire);
        }

        Debug.Log("old Sessions");
        foreach (Session newSess in liste_de_sessions)
        {
            Debug.Log(newSess.horaire);
        }

        liste_de_sessions = sessionList;
        return checkValidation();
    }

    public bool checkValidation()
    {
        int count = 0;
        foreach (Session session in liste_de_sessions)
        {
            if (session.faite)
            {
                count += 1;
            }
            else
            {
                return false;
            }
        }
        combo += 1;
        return true;
    }
}

[Serializable]
public class Session
{
    public DateTime horaire;
    public bool faite = false;

    public Session(DateTime horaire_de_session)
    {
        horaire = horaire_de_session;
    }

    public void end()
    {
        faite = true;
    }
}

[Serializable]
public class ObjectifJournalier
{
    public DateTime jour;
    public int objectif = 2;
    public int fait = 0;

    public ObjectifJournalier(Tache task)
    {
        jour = DateTime.Today;
        objectif = task.liste_de_sessions.Count;
        foreach (Session session in task.liste_de_sessions)
        {
            if (session.faite)
            {
                fait += 1;
            }
        }
    }
}
