
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

        public static EventArgs StartDialogueFactory(int dialogueID, int entryID) {
            EventArgs message = new EventArgs();
            message.variables = new object[2];
            message.variables[0] = dialogueID;
            message.variables[1] = entryID;
            return message;
        }

        public static void StartDialogueParser(EventArgs message, out int dialogueID, out int entryID) {
            dialogueID = (int)message.variables[0];
            entryID = (int)message.variables[1];
        }

        // TO-DO
        // Make EnemyStatistics and EnemyNarrative a complete collection of all parameters, instead of using single parameters
        // inside EnemyStatistics template. We will pass the structures only, and they will have every needed information
        public static void PossessionExecutedFactory(EnemyStatistics characterStatsInfo, EnemyNarrative characterNarrInfo)
        {
            EventArgs message = new EventArgs();
            message.variables = new object[2];
            message.variables[0] = characterStatsInfo;
            message.variables[1] = characterNarrInfo;
        }

        public static void PossessionExecutedParser(EventArgs message, out EnemyStatistics characterStatsInfo, EnemyNarrative characterNarrInfo)
        {
            characterStatsInfo = (EnemyStatistics)message.variables[0];
            characterNarrInfo = (EnemyNarrative)message.variables[1];
        }
    }

}