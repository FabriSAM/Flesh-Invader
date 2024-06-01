
namespace NotserializableEventManager {
    public static class EventArgsFactory {

        public static EventArgs LockPlayerFactory(bool lockValue) {
            EventArgs message = new EventArgs();
            message.variables = new object[1];
            message.variables[0] = lockValue;
            return message;
        }

        public static void LockPlayerParser(EventArgs message, out bool lockValue) {
            lockValue = (bool)message.variables[0];
        }

        public static EventArgs PlayerDeathFactory() {
            return new EventArgs();
        }

        public static void PlayerDeathParser(EventArgs message) {

        }

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
    }

}