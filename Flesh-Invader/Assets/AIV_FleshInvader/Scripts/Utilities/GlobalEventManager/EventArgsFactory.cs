
namespace NotserializableEventManager {
    public static class EventArgsFactory {

        #region LockPlayer
        public static EventArgs LockPlayerFactory(bool lockValue) {
            EventArgs message = new EventArgs();
            message.variables = new object[1];
            message.variables[0] = lockValue;
            return message;
        }

        public static void LockPlayerParser(EventArgs message, out bool lockValue) {
            lockValue = (bool)message.variables[0];
        }
        #endregion

        #region PlayerDeath
        public static EventArgs PlayerDeathFactory(Statistics statistics) {
            EventArgs message = new EventArgs();
            message.variables = new object[1];
            message.variables[0] = statistics;
            return message;
        }

        public static void PlayerDeathParser(EventArgs message, out Statistics statistics) {
            statistics = (Statistics)message.variables[0];
        }
        #endregion

        #region PlayerHealthUpdated
        public static EventArgs PlayerHealthUpdatedFactory(float maxHP, float currentHP) {
            EventArgs message = new EventArgs();
            message.variables = new object[2];
            message.variables[0] = maxHP;
            message.variables[1] = currentHP;
            return message;
        }

        public static void PlayerHealthUpdatedParser(EventArgs message, out float maxHP, out float currentHP) {
            maxHP = (float)message.variables[0];
            currentHP = (float)message.variables[1];
        }
        #endregion

        #region PlayerXpUpdated

        public static EventArgs PlayerXPUpdatedFactory(LevelStruct level)
        {
            EventArgs message = new EventArgs();
            message.variables = new object[1];
            message.variables[0] = level;
            return message;
        }

        public static void PlayerXPUpdatedParser(EventArgs message, out LevelStruct level)
        {
            level = (LevelStruct)message.variables[0];
        }
        #endregion

        #region StartDialogue
        public static EventArgs StartDialogueFactory(uint dialogueID, int entryID) {
            EventArgs message = new EventArgs();
            message.variables = new object[2];
            message.variables[0] = dialogueID;
            message.variables[1] = entryID;
            return message;
        }

        public static void StartDialogueParser(EventArgs message, out uint dialogueID, out int entryID) {
            dialogueID = (uint)message.variables[0];
            entryID = (int)message.variables[1];
        }
        #endregion

        #region DialoguePerformed
        public static EventArgs DialoguePerformedFactory(uint dialogueID)
        {
            EventArgs message = new EventArgs();
            message.variables = new object[1];
            message.variables[0] = dialogueID;
            return message;
        }

        public static void DialoguePerformedParser(EventArgs message, out uint dialogueID)
        {
            dialogueID = (uint)message.variables[0];
        }
        #endregion

        #region PossesionExecuted
        public static EventArgs PossessionExecutedFactory(EnemyInfo enemyInfo)
        {
            EventArgs message = new EventArgs();
            message.variables = new object[1];
            message.variables[0] = enemyInfo;
            return message;
        }

        public static void PossessionExecutedParser(EventArgs message, out EnemyInfo enemyInfo)
        {
            enemyInfo =  (EnemyInfo) message.variables[0];
        }
        #endregion

        #region MissionUpdated
        public static EventArgs MissionUpdatedFactory(Collectible collectible)
        {
            EventArgs message = new EventArgs();
            message.variables = new object[2];
            message.variables[0] = collectible;
            return message;
        }
        public static void MissionUpdatedParser(EventArgs message, out Collectible collectible)
        {
            collectible = (Collectible)message.variables[0];
        }
        #endregion

        #region PossessionAbilityState
        public static EventArgs PossessionAbilityStateFactory(bool state)
        {
            EventArgs message = new EventArgs();
            message.variables = new object[1];
            message.variables[0] = state;

            return message;
        }

        public static void PossessionAbilityStateParser(EventArgs message, out bool state)
        {
            state = (bool)message.variables[0];
        }
        #endregion

        #region CameraShake
        public static EventArgs CameraShakeFactory(float amplitude, float duration, bool overrideCoroutine)
        {
            EventArgs message = new EventArgs();
            message.variables = new object[3];
            message.variables[0] = amplitude;
            message.variables[1] = duration;
            message.variables[2] = overrideCoroutine;

            return message;
        }

        public static void CameraShakeParser(EventArgs message, out float amplitude, out float duration, out bool overrideCoroutine)
        {
            amplitude = (float)message.variables[0];
            duration = (float)message.variables[1];
            overrideCoroutine = (bool)message.variables[2];
        }
        #endregion

        #region CameraFOVChange
        public static EventArgs CameraFOVChangeFactory(float duration, float newFOV, bool overrideCoroutine)
        {
            EventArgs message = new EventArgs();
            message.variables = new object[3];
            message.variables[0] = duration;
            message.variables[1] = newFOV;
            message.variables[2] = overrideCoroutine;

            return message;
        }

        public static void CameraFOVParser(EventArgs message, out float duration, out float newFOV, out bool overrideCoroutine)
        {
            duration = (float)message.variables[0];
            newFOV = (float)message.variables[1];
            overrideCoroutine= (bool)message.variables[2];
        }
        #endregion
    }

}