mergeInto(LibraryManager.library, {
    TriggerIframeSwitch: function(index) {
        // Validate index as a number
        index = parseInt(index, 10);
        if (isNaN(index)) {
            console.error("Invalid index provided to TriggerIframeSwitch");
            return;
        }

        // Ensure the parent window exists
        if (typeof window.parent !== "undefined" && window.parent !== window) {
            try {
                // Post a message to the parent window with additional data
                window.parent.postMessage(
                    JSON.stringify({ action: "switchIframe", gameIndex: index }),
                    "*" // Replace '*' with the specific origin for better security, e.g., 'https://yourdomain.com'
                );
                console.log("Message sent to parent window with index:", index);
            } catch (err) {
                console.error("Error sending message to parent window:", err);
            }
        } else {
            console.warn("No parent window available for messaging.");
        }
    }
});
