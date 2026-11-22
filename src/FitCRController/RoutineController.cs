using FitCRModels;
using System.Globalization;

namespace FitCRController
{
    /// <summary>Controller for the routines, their exercises and the increases made to them.</summary>
    public class RoutineController
    {
        public RoutineController(InformationService informationService, NotificationController notificationController)
        {
            this.InformationService = informationService;
            this.NotificationController = notificationController;
        }

        public InformationService InformationService { get; set; }

        public NotificationController NotificationController { get; set; }

        /// <summary>Gets the routines of a user, from the newest to the oldest.</summary>
        public List<Routine> GetRoutines(string userId)
        {
            return this.InformationService.Routines
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.StartDate)
                .ToList();
        }

        /// <summary>Gets the last routines of a user.</summary>
        public List<Routine> GetLastRoutines(string userId, int count = 5)
        {
            return this.GetRoutines(userId).Take(count).ToList();
        }

        /// <summary>Gets the exercises of a routine.</summary>
        public List<Exercise> GetExercises(string routineId)
        {
            return this.InformationService.Exercises.Where(e => e.RoutineId == routineId).ToList();
        }

        /// <summary>Gets all the increases made in the routines of a user, from the newest to the oldest.</summary>
        public List<RoutineIncrease> GetIncreases(string userId)
        {
            var routineIds = this.InformationService.Routines.Where(r => r.UserId == userId).Select(r => r.Id).ToHashSet();
            return this.InformationService.RoutineIncreases
                .Where(i => routineIds.Contains(i.RoutineId))
                .OrderByDescending(i => i.Date)
                .ToList();
        }

        /// <summary>Adds a routine for a user.</summary>
        public OperationResult AddRoutine(string trainerId, string userId, DateTime startDate, DateTime endDate)
        {
            if (!this.InformationService.Trainers.Any(t => t.Id == trainerId))
            {
                return OperationResult.Fail("The trainer does not exist.");
            }

            if (!this.InformationService.Users.Any(u => u.Id == userId))
            {
                return OperationResult.Fail("The user does not exist.");
            }

            if (endDate.Date < startDate.Date)
            {
                return OperationResult.Fail("The end date cannot be before the start date.");
            }

            var id = this.InformationService.GenerateId("R", this.InformationService.Routines.Select(r => r.Id), 3);
            this.InformationService.Routines.Add(new Routine(id, userId, trainerId, FormatDate(startDate), FormatDate(endDate)));
            this.InformationService.Save(this.InformationService.Routines);
            this.NotificationController.Add(userId, "Nueva rutina agregada");
            return OperationResult.Ok("Routine added.", id);
        }

        /// <summary>Changes the dates of a routine.</summary>
        public OperationResult UpdateRoutine(string routineId, DateTime startDate, DateTime endDate)
        {
            var routine = this.InformationService.Routines.FirstOrDefault(r => r.Id == routineId);
            if (routine == null)
            {
                return OperationResult.Fail("The routine does not exist.");
            }

            if (endDate.Date < startDate.Date)
            {
                return OperationResult.Fail("The end date cannot be before the start date.");
            }

            routine.StartDate = startDate.Date;
            routine.EndDate = endDate.Date;
            this.InformationService.Save(this.InformationService.Routines);
            return OperationResult.Ok("Routine updated.");
        }

        /// <summary>Deletes a routine with its exercises and its increases.</summary>
        public OperationResult DeleteRoutine(string routineId)
        {
            if (this.InformationService.Routines.RemoveAll(r => r.Id == routineId) == 0)
            {
                return OperationResult.Fail("The routine does not exist.");
            }

            this.InformationService.Exercises.RemoveAll(e => e.RoutineId == routineId);
            this.InformationService.RoutineIncreases.RemoveAll(i => i.RoutineId == routineId);
            this.InformationService.Save(this.InformationService.Routines);
            this.InformationService.Save(this.InformationService.Exercises);
            this.InformationService.Save(this.InformationService.RoutineIncreases);
            return OperationResult.Ok("Routine deleted.");
        }

        /// <summary>Adds an exercise to a routine.</summary>
        public OperationResult AddExercise(string routineId, string name, int sets, int repetitions, double weight)
        {
            if (!this.InformationService.Routines.Any(r => r.Id == routineId))
            {
                return OperationResult.Fail("The routine does not exist.");
            }

            var error = this.ValidateExercise(name, sets, repetitions, weight);
            if (error != null)
            {
                return OperationResult.Fail(error);
            }

            var id = this.InformationService.GenerateId("E", this.InformationService.Exercises.Select(e => e.Id), 4);
            this.InformationService.Exercises.Add(new Exercise(
                id,
                routineId,
                name.Trim(),
                sets.ToString(CultureInfo.InvariantCulture),
                repetitions.ToString(CultureInfo.InvariantCulture),
                weight.ToString(CultureInfo.InvariantCulture)));

            this.InformationService.Save(this.InformationService.Exercises);
            return OperationResult.Ok("Exercise added.", id);
        }

        /// <summary>Changes an exercise. If the new weight is higher than the old one, the increase is recorded.</summary>
        public OperationResult UpdateExercise(string exerciseId, int sets, int repetitions, double weight)
        {
            var exercise = this.InformationService.Exercises.FirstOrDefault(e => e.Id == exerciseId);
            if (exercise == null)
            {
                return OperationResult.Fail("The exercise does not exist.");
            }

            var error = this.ValidateExercise(exercise.Name, sets, repetitions, weight);
            if (error != null)
            {
                return OperationResult.Fail(error);
            }

            if (weight > exercise.Weight)
            {
                this.RecordIncrease(exercise, weight);
            }

            exercise.Sets = sets;
            exercise.Repetitions = repetitions;
            exercise.Weight = weight;
            this.InformationService.Save(this.InformationService.Exercises);
            return OperationResult.Ok("Exercise updated.");
        }

        /// <summary>Deletes an exercise with its increases.</summary>
        public OperationResult DeleteExercise(string exerciseId)
        {
            if (this.InformationService.Exercises.RemoveAll(e => e.Id == exerciseId) == 0)
            {
                return OperationResult.Fail("The exercise does not exist.");
            }

            this.InformationService.RoutineIncreases.RemoveAll(i => i.ExerciseId == exerciseId);
            this.InformationService.Save(this.InformationService.Exercises);
            this.InformationService.Save(this.InformationService.RoutineIncreases);
            return OperationResult.Ok("Exercise deleted.");
        }

        private static string FormatDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private string? ValidateExercise(string name, int sets, int repetitions, double weight)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Contains(','))
            {
                return "The name is required and cannot contain commas.";
            }

            if (sets <= 0 || repetitions <= 0)
            {
                return "The sets and the repetitions must be greater than zero.";
            }

            return weight < 0 ? "The weight cannot be negative." : null;
        }

        private void RecordIncrease(Exercise exercise, double newWeight)
        {
            var id = this.InformationService.GenerateId("I", this.InformationService.RoutineIncreases.Select(i => i.Id), 3);
            this.InformationService.RoutineIncreases.Add(new RoutineIncrease(
                id,
                exercise.RoutineId,
                exercise.Id,
                FormatDate(DateTime.Today),
                exercise.Weight.ToString(CultureInfo.InvariantCulture),
                newWeight.ToString(CultureInfo.InvariantCulture)));

            this.InformationService.Save(this.InformationService.RoutineIncreases);
        }
    }
}
