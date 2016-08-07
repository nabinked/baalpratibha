// Write your Javascript code.
$(document).on('click', '.confirm-form-post', function (evt) {
    evt.preventDefault();
    var form = $(this).closest('form');
    confirm('Are you sure ?', '', true, form);

});

function confirm(title, message, closeIcon, form) {
    $.confirm({
        title: title,
        confirmButton: "Yes",
        cancelButton: "No",
        closeIcon: closeIcon,
        icon: 'fa fa-exclamation',
        confirm: function () {
            form.submit();
        },
        cancel: function () {
            console.log('Prompt canceled!');
        }
    });
};

$(document).on("change", ".img-file-input", function () {
    $(this).parent("form").submit();
});

$(document).on("click", ".upload-image-btn", function () {
    $(this).parent("form").find(".img-file-input").trigger("click");
});


