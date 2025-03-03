mergeInto(LibraryManager.library, {
    ExitGame: function() {
        // Ensure the parent window exists
        if (typeof window.parent !== "undefined" && window.parent !== window) {
            try {
                // Post a message to the parent window with additional data
                window.parent.postMessage(
                    JSON.stringify({ action: "quitGame"}),
                    "*" // Replace '*' with the specific origin for better security, e.g., 'https://yourdomain.com'
                );
                console.log("Message sent to parent window to quit game.");
            } catch (err) {
                console.error("Error sending message to parent window:", err);
            }
        } else {
            console.warn("No parent window available for messaging.");
        }
    }
});
