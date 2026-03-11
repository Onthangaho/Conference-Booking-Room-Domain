import React from "react";
import ReactDOM from "react-dom";

function ConfirmModal({ message, onConfirm, onCancel }) {
  return ReactDOM.createPortal(
    <div className="modal-overlay">
      <div className="modal-box">
        <p>{message}</p>
        <div className="modal-actions">
          <button className="btn danger" onClick={onConfirm}>
            Yes, Cancel
          </button>
          <button className="btn primary" onClick={onCancel}>
            No
          </button>
        </div>
      </div>
    </div>,
    document.body
  );
}

export default ConfirmModal;