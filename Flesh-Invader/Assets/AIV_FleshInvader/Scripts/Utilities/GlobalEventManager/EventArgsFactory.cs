
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
        public static EventArgs PlayerDeathFactory() {
            return new EventArgs();
        }

        public static void PlayerDeathParser(EventArgs message) {

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
        public static EventArgs PlayerXPUpdatedFactory(float maxXP, float currentXP) {
            EventArgs message = new EventArgs();
            message.variables = new object[2];
            message.variables[0] = maxXP;
            message.variables[1] = currentXP;
            return message;
        }

        public static void PlayerXPUpdatedParser(EventArgs message, out float maxXP, out float currentXP) {
            maxXP = (float)message.variables[0];
            currentXP = (float)message.variables[1];
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
        public static EventArgs MissionUpdatedFactory(int maxValue, int currentValue)
        {
            EventArgs message = new EventArgs();
            message.variables = new object[2];
            message.variables[0] = maxValue;
            message.variables[1] = currentValue;
            return message;
        }
        public static void MissionUpdatedParser(EventArgs message, out int maxValue, out int currentValue)
        {
            maxValue = (int)message.variables[0];
            currentValue = (int)message.variables[1];
        }
        #endregion
    }

}