using SuperplayerChallenge.Objects;
using Models;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Business
{
    public class ChallengeSolverBusiness : IDisposable
    {
        public static ChallengeSolverBusiness self;
        private Killer killer = null;
        private Detective detective = new Detective();
        private bool disposed = false;

        public static ChallengeSolverBusiness GetInstance()
        {
            if (self == null)
            {
                self = new ChallengeSolverBusiness();
            }
            return self;

        }
        
        public TheEndModel FindWhoIsTheKiller(ScenarioModel scenario, List<Witness> lWitness)
        {           
            if (scenario == null || scenario.Guns.Count == 0 || scenario.Suspects.Count == 0 || scenario.Locals.Count == 0)
                return null;

            TheEndModel theEnd = new TheEndModel();

            List<Killer> lKillers = new List<Killer>();
            detective.Guesses = new List<Killer>();

            for (int i = 0; i < lWitness.Count(); i++)
            {
                killer = null;
                FindTheKiller(0, 0, 0, lWitness[i], scenario);
                lKillers.Add(killer);
            }

            theEnd.Killers = lKillers;
            theEnd.Detective = detective;
            return theEnd ;
        }

        private void FindTheKiller(int idxGun, int idxLocal, int idxSuspect, Witness witness, ScenarioModel scenario)
        {
            int response = witness.IsHim(idxSuspect, idxLocal, idxGun);
            detective.Guesses.Add(new Killer() { Gun = scenario.Guns[idxGun], Local = scenario.Locals[idxLocal], Suspect = scenario.Suspects[idxLocal]});
            
            /* Wrong Suspect */
            if (response == 1)
            {
                //FindTheKiller(gun, local, scenario.Suspects[idxSuspect+1], witness, scenario, idxGun, idxLocal, idxSuspect+1);
                FindTheKiller(idxGun, idxLocal, idxSuspect+1, witness, scenario);
                return;
            }

            /* Wrong Local */
            else if (response == 2)
            {
                //FindTheKiller(gun, scenario.Locals[idxLocal + 1], suspect, witness, scenario, idxGun, idxLocal+1, idxSuspect);
                FindTheKiller(idxGun, idxLocal + 1, idxSuspect, witness, scenario);
                return;
            }

            /* Wrong Gun */
            else if (response == 3)
            {
                //FindTheKiller(scenario.Guns[idxGun + 1], local, suspect, witness, scenario, idxGun+1, idxLocal, idxSuspect);
                FindTheKiller(idxGun + 1, idxLocal, idxSuspect, witness, scenario);
                return;

            }

            killer = new Killer() { Gun = scenario.Guns[idxGun], Local = scenario.Locals[idxLocal], Suspect = scenario.Suspects[idxSuspect] };
            return;

        }

        public ScenarioModel DeserializeJsonFileScenario(Stream file)
        {
                /* Get contents to string */
                string str = (new StreamReader(file)).ReadToEnd();

                /* Deserializes string into object */
                Scenario scenario = JsonConvert.DeserializeObject<Scenario>(str);

                /* Create objects from the deserialized */
                return CreateScenario(scenario);
        }

        public List<Witness> DeserializeJsonFileWitness(Stream file, ScenarioModel scenario)
        {
            /* Get contents to string */
            string str = (new StreamReader(file)).ReadToEnd();

            /* Deserializes string into object */
            WitnessFileDownload response = JsonConvert.DeserializeObject<WitnessFileDownload>(str);


            /* Create objects from the deserialized json */
            return CreateWitness(response, scenario);
        }

        private ScenarioModel CreateScenario(Scenario scenario)
        {
            List<Suspect> suspects = new List<Suspect>();
            List<Gun> guns = new List<Gun>();
            List<Local> locals = new List<Local>();

            /* Create Suspects */
            for (int i = 0; i < scenario.Suspects.Length; i++)
            {
                suspects.Add(new Suspect() { Id = i + 1, Name = scenario.Suspects[i] });
            }

            /* Create Guns */
            for (int i = 0; i < scenario.Guns.Length; i++)
            {
                guns.Add(new Gun() { Id = i + 1, Name = scenario.Guns[i] });
            }

            /* Create Locals */
            for (int i = 0; i < scenario.Locals.Length; i++)
            {
                locals.Add(new Local() { Id = i + 1, Name = scenario.Locals[i] });
            }

            return new ScenarioModel() { Guns = guns, Suspects = suspects, Locals = locals };
        }

        private List<Witness> CreateWitness(WitnessFileDownload response, ScenarioModel scenario)
        {
            List<Witness> lWitness = new List<Witness>();
            List<ResponseFile> responseF = response.Scenario;
            for(int i=0; i<responseF.Count(); i++)
            //for(int i=0; i< response.responses.Count; i++)
                try
                {
                    {
                        lWitness.Add(new Witness() { Killer = new Killer() { Gun = scenario.Guns.Where(g => g.Name == responseF[i].Gun).FirstOrDefault(), Local = scenario.Locals.Where(l => l.Name == responseF[i].Local).FirstOrDefault(), Suspect = scenario.Suspects.Where(s => s.Name == responseF[i].Suspect).FirstOrDefault() } });
                    }
                }
                catch (Exception e)
                {
                    lWitness = null;
                }
            return lWitness;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (self != null)
                    {
                        self.Dispose();
                    }
                }
                disposed = true;
            }
        }
    }
}