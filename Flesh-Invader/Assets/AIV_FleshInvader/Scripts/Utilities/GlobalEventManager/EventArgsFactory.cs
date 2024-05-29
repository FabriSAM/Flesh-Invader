
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

        public static EventArgs PlayerHealthUpdatedFactory(int maxHP, int currentHP) {
            EventArgs message = new EventArgs();
            message.variables = new object[2];
            message.variables[0] = maxHP;
            message.variables[1] = currentHP;
            return message;
        }

        public static void PlayerHealthUpdatedParser(EventArgs message, out int maxHP, out int currentHP) {
            maxHP = (int)message.variables[0];
            currentHP = (int)message.variables[1];
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

    }

}