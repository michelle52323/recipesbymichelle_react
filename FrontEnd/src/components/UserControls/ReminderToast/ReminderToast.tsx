import React from 'react';
import './remindertoast.css';

import Icon  from '../../UserControls/Icons/icons';


interface ReminderToastProps {
    message: React.ReactNode;

    onClose?: () => void;
}

function ReminderToast({ message, onClose }: ReminderToastProps) {

    return (
        <div className="reminder-toast">
            <div className="reminder-toast-icon">
                <Icon name="info" />

            </div>

            <div className="reminder-toast-message">
                {message}
            </div>

            {onClose && (
                <button
                    type="button"
                    className="reminder-toast-close"
                    onClick={onClose}
                    aria-label="Close reminder"
                >
                    ×
                </button>
            )}
        </div>
    );
}

export default ReminderToast;
