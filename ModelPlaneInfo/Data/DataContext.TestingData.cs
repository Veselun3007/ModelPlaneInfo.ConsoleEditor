using ModelPlaneInfo.Entities;
using System;
using System.Linq;

namespace ModelPlaneInfo.Data
{
    /// <summary>
    /// A class designed for creating test data,
    /// which are used for further work
    /// </summary>
    public partial class DataContext
    {
        /// <summary>
        /// Creating data that represents
        /// entity "Aircraft Types"
        /// </summary>
        public void CreatTypePlanes()
        {
            PlaneTypes.Add(new PlaneType("Destroyer") { Id = 1 });
            PlaneTypes.Add(new PlaneType("Scout") { Id = 2 });
            PlaneTypes.Add(new PlaneType("Stormtrooper") { Id = 3 });
            PlaneTypes.Add(new PlaneType("Passenger's") { Id = 4 });
            PlaneTypes.Add(new PlaneType("Cargo") { Id = 5 });
            PlaneTypes.Add(new PlaneType("FirePlane", "A subtype of civil aviation") { Id = 6 });
            PlaneTypes.Add(new PlaneType("Sanitary", "A subtype of civil aviation") { Id = 7 });
        }

        /// <summary>
        /// Creating data that represents
        /// entity "Airplane Models"
        /// </summary>
        public void CreateModelsPlane()
        {
            ModelPlanes.Add(new ModelPlane("Boeing 737",
                PlaneTypes.First(e => e.Name == "Passenger's"), 2002, "",
                "Boeing 737 — short-medium-haul " +
                "twin-engine narrow-body passenger " +
                "jet aircraft that was developed and " +
                "manufactured by Boeing Commercial " +
                "Airplanes. In March 2018, the company " +
                "produced the 10,000th aircraft of this model ",
                "The most massive jet aircraft, " +
                "that is produced in the entire history of passenger aircraft construction")
            { Id = 1 });

            ModelPlanes.Add(new ModelPlane("F-16 Fighting Falcon",
               PlaneTypes.First(e => e.Name == "Destroyer"), null, "Yes",
               "", "The F-16 is no longer being built for the US Air Force, " +
               "it is still produced for export")
            { Id = 2 });

            ModelPlanes.Add(new ModelPlane("Ан-32П",
               PlaneTypes.First(e => e.Name == "FirePlane"), 1993, "Unknow",
               "Aн-32П is a Ukrainian firefighting aircraft developed " +
               "based on An-32. The plane is equipped with 4 external " +
               "removable tanks with a volume of 2000 l each. " +
               "Discharge of fire extinguishing liquid can be carried out " +
               "from all sides simultaneously or alternately " +
               "with a given interval.",
               "")
            { Id = 3 });

            ModelPlanes.Add(new ModelPlane("Ан-225 'Мрія'",
               PlaneTypes.First(e => e.Name == "Cargo"), 2010, "No",
               "Ан-225 «Мрія» — the largest and most powerful " +
               "transport aircraft in the world created by Kyiv" +
               "KB named after Antonov. The chief designer is Viktor Ilyich Tolmachov.",
               "Ан-225 with a maximum take-off weight of 640t, it is the heaviest aircraft in the world.")
            { Id = 4 });
        }

        /// <summary>
        /// General data creation method,
        /// which represent the description of the "Airplane models" software
        /// </summary>
        public void CreateTestData()
        {
            CreatTypePlanes();
            CreateModelsPlane();
        }
    }
}
