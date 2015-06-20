using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;

namespace LecznaHub.Helpers
{
    public static class BackgroundTasksHelper
    {
        //
        // Register a background task with the specified taskEntryPoint, name, trigger,
        // and condition (optional).
        //
        // taskEntryPoint: Task entry point for the background task.
        // taskName: A name for the background task.
        // trigger: The trigger for the background task.
        // condition: Optional parameter. A conditional event that must be true for the task to fire.
        //
        public static BackgroundTaskRegistration RegisterBackgroundTask(string taskEntryPoint,
            string taskName,
            IBackgroundTrigger trigger,
            IBackgroundCondition condition)
        {
            //
            // Check for existing registrations of this background task.
            //

            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {

                if (cur.Value.Name == taskName)
                {
                    // 
                    // The task is already registered.
                    // 

                    return (BackgroundTaskRegistration) (cur.Value);
                }
            }


            //
            // Register the background task.
            //

            var builder = new BackgroundTaskBuilder();

            builder.Name = taskName;
            builder.TaskEntryPoint = taskEntryPoint;
            builder.SetTrigger(trigger);

            if (condition != null)
            {

                builder.AddCondition(condition);
            }

            BackgroundTaskRegistration task = builder.Register();

            return task;
        }

        public static async void RegisterLiveTileUpdaterTask()
        {
            try
            {
                BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();
                if (status == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity ||
                    status == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity)
                {
                    //bool isRegistered = BackgroundTaskRegistration.AllTasks.Any(x => x.Value.Name == "Notification task");
                    //if (!isRegistered)
                    //{

                    TimeTrigger myTimeTrigger = new TimeTrigger(30, false);

                    BackgroundTaskRegistration task = Helpers.BackgroundTasksHelper.RegisterBackgroundTask(
                        "LecznaHub.BackgroundTasks.TileUpdateTask",
                        "Live tile updater", myTimeTrigger, null);
                    //}
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("The access has already been granted");
            }
        }
    }
}
