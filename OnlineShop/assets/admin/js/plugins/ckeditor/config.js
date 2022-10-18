/**
 * @license Copyright (c) 2003-2022, CKSource Holding sp. z o.o. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
    config.syntaxhighlight_lang = 'csharp';
    config.syntaxhighlight_hideControls = true;
    config.language = 'vi';
    config.filebrowserBrowseUrl = '/assets/admin/js/plugins/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = '/assets/admin/js/plugins/ckfinder.html?Type=Images';
    config.filebrowserFlashBrowseUrl = '/assets/admin/js/plugins/ckfinder.html?Type=Flash';
    config.filebrowserUploadUrl = '/assets/admin/js/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/Data';
    config.filebrowserFlashUploadUrl = '/assets/admin/js/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';

    CKFinder.setupCKEditor(null, '/assets/admin/js/plugins/ckfinder/');
};

//CKEDITOR.editorConfig = function (config) {
//    // Define changes to default configuration here.
//    // For the complete reference:
//    // http://docs.ckeditor.com/#!/api/CKEDITOR.config

//    // The toolbar groups arrangement, optimized for a single toolbar row.
//    config.toolbarGroups = [
//        { name: 'document', groups: ['mode', 'document', 'doctools'] },
//        { name: 'clipboard', groups: ['clipboard', 'undo'] },
//        { name: 'editing', groups: ['find', 'selection', 'spellchecker'] },
//        { name: 'forms' },
//        { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
//        { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'] },
//        { name: 'links' },
//        { name: 'insert' },
//        { name: 'styles' },
//        { name: 'colors' },
//        { name: 'tools' },
//        { name: 'others' },
//        { name: 'about' }
//    ];

//    // The default plugins included in the basic setup define some buttons that
//    // we don't want too have in a basic editor. We remove them here.
//    config.removeButtons = 'Cut,Copy,Paste,Undo,Redo,Anchor,Underline,Strike,Subscript,Superscript';

//    // Let's have it basic on dialogs as well.
//    config.removeDialogTabs = 'link:advanced';
//};
