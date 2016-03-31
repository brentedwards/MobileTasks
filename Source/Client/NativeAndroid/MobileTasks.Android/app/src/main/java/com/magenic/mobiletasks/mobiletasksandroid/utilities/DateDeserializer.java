package com.magenic.mobiletasks.mobiletasksandroid.utilities;

import com.google.gson.JsonDeserializationContext;
import com.google.gson.JsonDeserializer;
import com.google.gson.JsonElement;
import com.google.gson.JsonParseException;

import java.lang.reflect.Type;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Arrays;
import java.util.Date;
import java.util.Locale;

/**
 * Created by kevinf on 3/29/2016.
 */
public class DateDeserializer implements JsonDeserializer<Date> {

    private String[] dateFormats;

    public DateDeserializer(String[] dateFormate) {
        this.dateFormats = dateFormate;
    }

    @Override
    public Date deserialize(JsonElement jsonElement, Type typeOF,
                            JsonDeserializationContext context) throws JsonParseException {

        for (String format : dateFormats) {
            try {
                return new SimpleDateFormat(format, Locale.US).parse(jsonElement.getAsString());
            } catch (ParseException e) {
            }
        }
        throw new JsonParseException("Unparseable date: \"" + jsonElement.getAsString()
                + "\". Supported formats: " + Arrays.toString(dateFormats));
    }
}