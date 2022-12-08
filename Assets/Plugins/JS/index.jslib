mergeInto(LibraryManager.library, {
    SendFBInstantRequestToJS: function (requestId, type, methodName, paramsRequest) {
        window.fetchRequestFbInstant(
            UTF8ToString(requestId),
            UTF8ToString(type),
            UTF8ToString(methodName),
            UTF8ToString(paramsRequest)
        );
    },
});
