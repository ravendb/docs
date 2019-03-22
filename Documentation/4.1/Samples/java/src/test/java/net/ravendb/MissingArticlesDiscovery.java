package net.ravendb;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import java.util.stream.Collectors;

public class MissingArticlesDiscovery {

    public static void main(String[] args) throws IOException {
        File file = new File("../../");
        String canonicalPath = file.getCanonicalPath();

        Collection<String> all = new ArrayList<>();
        addTree(new File(canonicalPath), all);

        String dotNetPostfix = ".dotnet.markdown";
        String javaPostfix = ".java.markdown";

        List<String> dotnet = all
            .stream()
            .filter(x -> x.endsWith(dotNetPostfix))
            .map(x -> x.substring(0, x.length() - dotNetPostfix.length()))
            .collect(Collectors.toList());

        List<String> java = all
            .stream()
            .filter(x -> x.endsWith(javaPostfix))
            .map(x -> x.substring(0, x.length() - javaPostfix.length()))
            .collect(Collectors.toList());

        List<String> pending = dotnet
            .stream()
            .filter(x -> !java.contains(x))
            .collect(Collectors.toList());

        for (String item : pending) {
            System.out.println(item);
        }

        System.out.println("Total dotnet: " + dotnet.size());
        System.out.println("Total java: " + java.size());
        System.out.println("Remaining java: " + pending.size());
    }

    static void addTree(File file, Collection<String> all) throws IOException {
        File[] children = file.listFiles();
        if (children != null) {
            for (File child : children) {
                all.add(child.getCanonicalPath());
                addTree(child, all);
            }
        }
    }
}
