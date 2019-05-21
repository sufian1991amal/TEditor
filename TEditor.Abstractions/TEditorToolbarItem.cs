using System;
using System.Collections.Generic;
using System.Linq;

namespace TEditor.Abstractions
{
    public class ToolbarBuilder : List<TEditorToolbarItem>
    {
        public ToolbarBuilder AddBasic()
        {
            AddBold();
            AddItalic();
            AddUnderline();
            AddRemoveFormat();
            return this;
        }

        public ToolbarBuilder AddStandard()
        {
            AddBasic();
            AddJustifyCenter();
            AddJustifyFull();
            AddJustifyLeft();
            AddJustifyRight();
            AddH1();
            AddH2();
            AddH3();
            AddH4();
            AddH5();
            AddH6();
            AddTextColor();
            //BackgroundColor
            AddUnorderedList();
            AddOrderedList();
            return this;
        }

        public ToolbarBuilder AddAll()
        {
            AddMentioning();
            AddAttachment();
            AddStandard();
            AddSubscript();
            AddSuperscript();
            AddStrikeThrough();
            AddHorizontalRule();
            AddIndent();
            AddOutdent();
            //insertImage
            //insertLink
            //removeLink
            //QuickLink
            AddUndo();
            AddRedo();
            AddParagraph();
            return this;
        }

        #region Add functions

        public ToolbarBuilder AddMentioning(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "email.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.Mentioning,
                Label = "mentioning",
                ClickFunc = (input) =>
                {
                    return string.Empty;
                }
            });
            return this;
        }

        public ToolbarBuilder AddAttachment(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "hashtag.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.Attachment,
                Label = "attachment",
                ClickFunc = (input) =>
                {
                    return string.Empty;
                }
            });
            return this;
        }

        public ToolbarBuilder AddBold(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSbold.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.Bold,
                Label = "bold",
                ClickFunc = (input) =>
                {
                    input.SetBold();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddItalic(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSitalic.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.Italic,
                Label = "italic",
                ClickFunc = (input) =>
                {
                    input.SetItalic();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddSubscript(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSsubscript.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.SubScript,
                Label = "subscript",
                ClickFunc = (input) =>
                {
                    input.SetSubscript();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddSuperscript(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSsuperscript.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.SuperScript,
                Label = "superscript",
                ClickFunc = (input) =>
                {
                    input.SetSuperscript();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddStrikeThrough(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSstrikethrough.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.StrikeThrough,
                Label = "strikethrough",
                ClickFunc = (input) =>
                {
                    input.SetStrikeThrough();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddUnderline(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSunderline.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.UnderLine,
                Label = "underline",
                ClickFunc = (input) =>
                {
                    input.SetUnderline();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddRemoveFormat(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSclearstyle.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.ClearStyle,
                Label = "clearstyle",
                ClickFunc = (input) =>
                {
                    input.RemoveFormat();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddJustifyLeft(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSleftjustify.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.LeftJustify,
                Label = "leftjustify",
                ClickFunc = (input) =>
                {
                    input.AlignLeft();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddJustifyCenter(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSScenterjustify.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.CenterJustify,
                Label = "centerjustify",
                ClickFunc = (input) =>
                {
                    input.AlignCenter();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddJustifyRight(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSrightjustify.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.RightJustify,
                Label = "rightjustify",
                ClickFunc = (input) =>
                {
                    input.AlignRight();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddJustifyFull(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSforcejustify.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.ForceJustify,
                Label = "forcejustify",
                ClickFunc = (input) =>
                {
                    input.AlignFull();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddH1(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSh1.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.H1,
                Label = "h1",
                ClickFunc = (input) =>
                {
                    input.Heading1();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddH2(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSh2.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.H2,
                Label = "h2",
                ClickFunc = (input) =>
                {
                    input.Heading2();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddH3(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSh3.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.H3,
                Label = "h3",
                ClickFunc = (input) =>
                {
                    input.Heading3();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddH4(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSh4.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.H4,
                Label = "h4",
                ClickFunc = (input) =>
                {
                    input.Heading4();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddH5(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSh5.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.H5,
                Label = "h5",
                ClickFunc = (input) =>
                {
                    input.Heading5();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddH6(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSh6.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.H6,
                Label = "h6",
                ClickFunc = (input) =>
                {
                    input.Heading6();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddUnorderedList(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSunorderedlist.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.UnorderedList,
                Label = "unorderedlist",
                ClickFunc = (input) =>
                {
                    input.SetUnorderedList();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddOrderedList(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSorderedlist.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.OrderedList,
                Label = "orderedlist",
                ClickFunc = (input) =>
                {
                    input.SetOrderedList();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddHorizontalRule(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSShorizontalrule.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.HorizontalRule,
                Label = "horizontalrule",
                ClickFunc = (input) =>
                {
                    input.SetHR();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddIndent(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSindent.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.Indent,
                Label = "indent",
                ClickFunc = (input) =>
                {
                    input.SetIndent();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddOutdent(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSoutdent.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.Outdent,
                Label = "outdent",
                ClickFunc = (input) =>
                {
                    input.SetOutdent();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddQuickLink(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSquicklink.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.QuickLink,
                Label = "quicklink",
                ClickFunc = (input) =>
                {
                    input.QuickLink();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddUndo(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSundo.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.Undo,
                Label = "undo",
                ClickFunc = (input) =>
                {
                    input.Undo();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddRedo(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSredo.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.Redo,
                Label = "redo",
                ClickFunc = (input) =>
                {
                    input.Redo();
                    return string.Empty;
                }
            });
            return this;
        }


        public ToolbarBuilder AddParagraph(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSSparagraph.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.Paragraph,
                Label = "paragraph",
                ClickFunc = (input) =>
                {
                    input.Paragraph();
                    return string.Empty;
                }
            });
            return this;
        }

        public ToolbarBuilder AddTextColor(string imagePath = null)
        {
            AddOnce(new TEditorToolbarItem
            {
                ImagePath = string.IsNullOrEmpty(imagePath) ? "ZSStextcolor.png" : imagePath,
                Action = TEditorToolbarItem.EnumAction.TextColor,
                Label = "textcolor",
                ClickFunc = (input) =>
                {
                    if (input.LaunchColorPicker != null)
                    {
                        input.PrepareInsert();
                        input.LaunchColorPicker.Invoke();
                    }
                    return string.Empty;
                }
            });
            return this;
        }

        #endregion

        void AddOnce(TEditorToolbarItem item)
        {
            if (this.Count == 0)
            {
                this.Add(item);
                return;
            }
            var iteminlist = this.FirstOrDefault(t => t.Action == item.Action);
            if (iteminlist == null)
                this.Add(item);
        }
    }

    public class TEditorToolbarItem
    {
        public EnumAction Action { get; set; }

        public string ImagePath { get; set; }

        public string Label { get; set; }

        internal Func<ITEditorAPI, string> ClickFunc { get; set; }

        public enum EnumAction
        {
            Mentioning,
            Attachment,
            Bold,
            Italic,
            SubScript,
            SuperScript,
            StrikeThrough,
            UnderLine,
            ClearStyle,
            LeftJustify,
            CenterJustify,
            RightJustify,
            ForceJustify,
            H1,
            H2,
            H3,
            H4,
            H5,
            H6,
            UnorderedList,
            OrderedList,
            HorizontalRule,
            Indent,
            Outdent,
            QuickLink,
            Undo,
            Redo,
            Paragraph,
            TextColor
        }
    }

}

