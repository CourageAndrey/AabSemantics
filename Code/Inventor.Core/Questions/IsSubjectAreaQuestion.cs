namespace Inventor.Core.Questions
{
    [QuestionDescriptor]
    public sealed class IsSubjectAreaQuestion : Question
    {
        [PropertyDescriptor(true, "QuestionNames.ParamConcept")]
        public Concept Concept
        { get; set; }

        [PropertyDescriptor(true, "QuestionNames.ParamArea")]
        public Concept Area
        { get; set; }
    }
}
