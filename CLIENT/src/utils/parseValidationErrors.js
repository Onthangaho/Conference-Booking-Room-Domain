export function parseValidationErrors(problemDetails) {
  const errors = {};

  if (problemDetails?.errors) {
    Object.entries(problemDetails.errors).forEach(([field, messages]) => {
      errors[field] = Array.isArray(messages) ? messages.join(", ") : String(messages);
    });
  }

  if (Object.keys(errors).length === 0 && problemDetails?.message) {
    errors.general = problemDetails.message;
  }

  return errors;
}