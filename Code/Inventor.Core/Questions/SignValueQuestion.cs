namespace Inventor.Core.Questions
{
    [QuestionDescriptor]
    public sealed class SignValueQuestion : Question
    {
        [PropertyDescriptor(true, "QuestionNames.ParamConcept")]
        public Concept Concept
        { get; set; }

        [PropertyDescriptor(true, "QuestionNames.ParamSign")]
        public Concept Sign
        { get; set; }
    }
}
